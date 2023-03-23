    
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
    [Serializable]
    public class Choice
    {
        public int index;
        public Message message;
        public string finish_reason;
    }

    [Serializable]
    public class ChatResponse
    {
        public string id;
        public int created;
        public List<Choice> choices;
    }

    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class ChatParameters
    {
        public string model;
        public List<Message> messages;
        public float temperature = 1.0f;
        public float top_p = 1.0f;
        public int n = 1;
        public bool stream = false;
        public int max_tokens = int.MaxValue;
        public float presence_penalty = 0.0f;
        public float frequency_penalty = 0.0f;
    }

    public class ChatOpenAIRequest : BaseOpenAIRequest<ChatResponse>
    {
        protected ChatParameters parameters;

        public ChatOpenAIRequest(string apiKey, ChatParameters parameters) : base(apiKey) 
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