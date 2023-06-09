using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    [CreateAssetMenu(menuName = "RecursiveRhapsody/Basic/ImageVariationGeneratorSettings")]
    public class BasicImageVariationGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ImageResponse>
    {
        public Texture2D ReferenceAsset;
        public Texture2D ResultAsset;
        public DefaultAsset OutputDirectory;
        public ImageSelectionSize ImageSize = ImageSelectionSize.x1024x1024;
        [Range(1,4)]
        public int NumberOfImages = 1;

        public void SendRequest(Action<ImageResponse> action = null, Action<string> error = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var size = BasicImageSizes.ConvertFromEnum(ImageSize);
            var request = new ImageVariationOpenAIRequest(APIKey, new ImageVariationParameters () {
                image = ReferenceAsset,
                n = NumberOfImages,
            });

            request.SendRequest(action, error);
        }
    }
}