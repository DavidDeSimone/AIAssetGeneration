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
    public class ChatModel
    {
        public const string ChatGPT_3_5 = "gpt-3.5-turbo";
        public const string Davinci_3 = "text-davinci-003";
    }

    public class Roles
    {
        public const string User = "user";
        public const string System = "system";
        public const string Assistant = "assistant";
    }

    public interface IOpenAIRequest {
        public IEnumerator Request();
    }
    public abstract class BaseOpenAIRequest : IOpenAIRequest
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

        public static void StartBackgroundTask(IEnumerator update, Action end = null)
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
                            end();
                        }

                        EditorApplication.update -= closureCallback;
                    }
                }
                catch (Exception ex)
                {
                    if (end != null)
                    {
                        end();
                    }

                    Debug.LogException(ex);
                    EditorApplication.update -= closureCallback;
                }
            };

            EditorApplication.update += closureCallback;
        }

        public void SendRequest()
        {
            StartBackgroundTask(Request());
        }

        public IEnumerator Request()
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

                // EditorUtility.ClearProgressBar();

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
                        // var responseJson = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                        // callback(responseJson);
                        break;
                }
            }

        }
    }

    public class ChatOpenAIRequest : BaseOpenAIRequest 
    {
        [Serializable]
        public class Message
        {
            public string role;
            public string content;
        }
        // @TODO make this a builder pattern
        [Serializable]
        public class Parameters
        {
            public string model;
            public List<Message> messages;
        }

        protected ChatOpenAIRequest.Parameters parameters;

        public ChatOpenAIRequest(string apiKey, ChatOpenAIRequest.Parameters parameters) : base(apiKey) 
        {
            this.parameters = parameters;
        }

        protected override UnityWebRequest getWebRequest()
        {
            var json = JsonUtility.ToJson(parameters);
            Debug.Log(json);
            var request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);

            return request;
        }
    }
}