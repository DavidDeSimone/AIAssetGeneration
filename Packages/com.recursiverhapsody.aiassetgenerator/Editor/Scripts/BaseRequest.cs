using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;

namespace com.recursiverhapsody
{
    public interface IOpenAIRequest<T> 
    where T: class
    {
        public void SendRequest(Action<T> action);
    }
    public abstract class BaseOpenAIRequest<T> : IOpenAIRequest<T>
    where T: class
    {
        protected string apiKey;
        public BaseOpenAIRequest(string apiKey)
        {
            this.apiKey = apiKey;
        }

        protected virtual void setContentHeaders(UnityWebRequest unityWebRequest)
        {
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        }

        protected abstract UnityWebRequest getWebRequest();

        public static void StartBackgroundTask<TReturn>(IEnumerator update, Action<TReturn> end = null)
        where TReturn: class
        {
            EditorApplication.CallbackFunction closureCallback = null;

            closureCallback = () =>
            {
                try
                {
                    if (update.MoveNext() == false)
                    {
                        if (end != null)
                        {
                            end(update.Current as TReturn);
                        }

                        EditorApplication.update -= closureCallback;
                    }
                }
                catch (Exception ex)
                {
                    // if (end != null)
                    // {
                        // end(ex.ToString());
                    // }

                    Debug.LogException(ex);
                    EditorApplication.update -= closureCallback;
                }
            };

            EditorApplication.update += closureCallback;
        }

        public void SendRequest(Action<T> action = null)
        {
            StartBackgroundTask<T>(Request(), action);
        }

        private IEnumerator Request()
        {
            using (var webRequest = getWebRequest()) {
                setContentHeaders(webRequest);
                webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                webRequest.disposeUploadHandlerOnDispose = true;
                webRequest.disposeDownloadHandlerOnDispose = true;

                yield return webRequest.SendWebRequest();

                while (webRequest.isDone == false)
                {
                    yield return null;
                }

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError("Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("HTTP Error: " + webRequest.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(webRequest.downloadHandler.text);
                        yield return JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                        break;
                }
            }

        }

        public static void RequestImage(string url, Action<Texture2D> action = null)
        {
            StartBackgroundTask(RequestImage(url), action);
        }

        private static IEnumerator RequestImage(string url)
        {
            var webRequest = UnityWebRequestTexture.GetTexture(url);
            yield return webRequest.SendWebRequest();
            while (webRequest.isDone == false)
            {
                yield return null;
            }
            
            if(webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                yield return ((DownloadHandlerTexture) webRequest.downloadHandler).texture;
            }
                // YourRawImage.texture = ((DownloadHandlerTexture) webRequest.downloadHandler).texture;
        }
    }
}