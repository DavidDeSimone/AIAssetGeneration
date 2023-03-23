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
    [Serializable]
    public class ImageVariationParameters
    {
        public Texture2D image;
        public int n = 1;
        public string size = BasicImageSizes.ImageSize_1024_1024;
        public string response_format = ImageResponseFormat.Url;
    }

    public class ImageVariationOpenAIRequest : BaseOpenAIRequest<ImageResponse>
    {
        protected ImageVariationParameters parameters;

        public ImageVariationOpenAIRequest(string apiKey, ImageVariationParameters parameters) : base(apiKey) 
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
            form.AddField("n", parameters.n);
            form.AddField("size", parameters.size);
            form.AddField("response_format", parameters.response_format);

            var request = UnityWebRequest.Post("https://api.openai.com/v1/images/variations", form);            
            return request;
        }
    }
}