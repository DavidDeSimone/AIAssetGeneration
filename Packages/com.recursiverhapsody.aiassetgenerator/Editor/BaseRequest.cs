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

        public static void StartBackgroundTask(IEnumerator update, Action<T> end = null)
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
                            end(update.Current as T);
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
            StartBackgroundTask(Request(), action);
        }

        private IEnumerator Request()
        {
            Debug.Log("Requesting.....");

            using (var webRequest = getWebRequest()) {
                setContentHeaders(webRequest);
                webRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
                webRequest.disposeUploadHandlerOnDispose = true;
                webRequest.disposeDownloadHandlerOnDispose = true;

                Debug.Log("Sending Request...");
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
                        yield return webRequest.downloadHandler.text;
                        // var responseJson = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                        // callback(responseJson);
                        break;
                }
            }

        }
    }
}