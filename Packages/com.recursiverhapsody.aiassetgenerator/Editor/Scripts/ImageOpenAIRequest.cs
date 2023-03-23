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
    public class ImageData
    {
        public string url;
        public string b64_json;
    }

    [Serializable]
    public class ImageResponse
    {
        public int created;
        public List<ImageData> data;
    }

    [Serializable]
    public class ImageParameters
    {
        public string prompt;
        public int n = 1;
        public string size = BasicImageSizes.ImageSize_1024_1024;
        public string response_format = ImageResponseFormat.Url;
    }

    public class ImageOpenAIRequest : BaseOpenAIRequest<ImageResponse>
    {
        protected ImageParameters parameters;

        public ImageOpenAIRequest(string apiKey, ImageParameters parameters) : base(apiKey) 
        {
            this.parameters = parameters;
        }

        protected override UnityWebRequest getWebRequest()
        {
            var json = JsonUtility.ToJson(parameters);
            Debug.Log(json);
            var request = new UnityWebRequest("https://api.openai.com/v1/images/generations", "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            return request;
        }
    }
}