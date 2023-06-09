using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    [CreateAssetMenu(menuName = "RecursiveRhapsody/Basic/ImageGeneratorSettings")]
    public class BasicImageGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ImageResponse>
    {
        public Texture2D ResultAsset;
        public DefaultAsset OutputDirectory;
        public ImageSelectionSize ImageSize = ImageSelectionSize.x1024x1024;
        [TextArea(3, 10)]
        public string Prompt;

        public void SendRequest(Action<ImageResponse> action = null, Action<string> error = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var size = BasicImageSizes.ConvertFromEnum(ImageSize);
            var request = new ImageOpenAIRequest(APIKey, new ImageParameters () {
                prompt = Prompt,
                size = size,
            });

            request.SendRequest(action, error);
        }
    }
}