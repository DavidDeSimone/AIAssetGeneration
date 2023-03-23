using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using Unity.Collections;
using System.IO;

namespace com.recursiverhapsody
{
    // [Serializable]
    // public class ImageData
    // {
    //     public string url;
    //     public string b64_json;
    // }

    // [Serializable]
    // public class ImageResponse
    // {
    //     public int created;
    //     public List<ImageData> data;
    // }

    // [Serializable]
    // public class Message
    // {
    //     public string role;
    //     public string content;
    // }

    /*
{
  "created": 1679371410,
  "data": [
    {
      "url": "https://oaidalleapiprodscus.blob.core.windows.net/private/org-twkJpIyjQwyI4OLwbJLC3Hwc/user-s3Dsdveu7dZPegmFtCkX6Kgz/img-h77l3bn2ojPvi89fvBMrBWyI.png?st=2023-03-21T03%3A03%3A30Z&se=2023-03-21T05%3A03%3A30Z&sp=r&sv=2021-08-06&sr=b&rscd=inline&rsct=image/png&skoid=6aaadede-4fb3-4698-a8f6-684d7786b067&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2023-03-21T03%3A41%3A03Z&ske=2023-03-22T03%3A41%3A03Z&sks=b&skv=2021-08-06&sig=X0JKC4ir5K9AzvzYrVh9VPaSRmQ9i/Gw1PG7kI6aaRM%3D"
    }
  ]
}
*/

    [Serializable]
    public class ImageEditParameters
    {
        public string prompt;
        public Texture2D image;
        public Texture2D mask;
        public int n = 1;
        public string size = BasicImageSizes.ImageSize_1024_1024;
        public string response_format = ImageResponseFormat.Url;

        // public int n = 1;
        // public string size = BasicImageSizes.ImageSize_1024_1024;
        // public string response_format = ImageResponseFormat.Url;
    }

    public class ImageEditOpenAIRequest : BaseOpenAIRequest<ImageResponse>
    {
        protected ImageEditParameters parameters;

        public ImageEditOpenAIRequest(string apiKey, ImageEditParameters parameters) : base(apiKey) 
        {
            this.parameters = parameters;
        }

        protected override void setContentHeaders(UnityWebRequest unityWebRequest)
        {
            unityWebRequest.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        }

        protected override UnityWebRequest getWebRequest()
        {
            var bytes = File.ReadAllBytes(AssetDatabase.GetAssetPath(parameters.image));
            var form = new WWWForm();
            form.AddBinaryData("image", bytes, "image.png", "image/png");
            form.AddField("prompt", parameters.prompt);

            if (parameters.mask != null) {
                var maskBytes = File.ReadAllBytes(AssetDatabase.GetAssetPath(parameters.mask));
                form.AddBinaryData("mask", maskBytes, "mask.png", "image/png");
            }
            form.AddField("n", parameters.n);
            form.AddField("size", parameters.size);
            form.AddField("response_format", parameters.response_format);

            var request = UnityWebRequest.Post("https://api.openai.com/v1/images/edits", form);
            
            // var bodyRaw = Encoding.UTF8.GetBytes(json);
            // request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            return request;
        }
    }
}