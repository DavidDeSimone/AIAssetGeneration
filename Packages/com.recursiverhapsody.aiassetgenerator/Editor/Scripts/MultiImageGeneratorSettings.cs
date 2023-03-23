using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    public enum ImageSelectionSize
    {
        x1024x1024,
        x512x512,
        x256x256,
    }


    [CreateAssetMenu(menuName = "RecursiveRhapsody/Advanced/MultiImageGeneratorSettings")]
    public class MultiImageGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ImageResponse>
    {
        [Range(1,4)]
        public int NumberOfImages;
        public ImageSelectionSize ImageSize;
        public DefaultAsset OutputDirectory;
        [TextArea(3, 10)]
        public string Prompt;

        public void SendRequest(Action<ImageResponse> action = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var size = BasicImageSizes.ImageSize_1024_1024;
            switch (ImageSize)
            {
                case ImageSelectionSize.x1024x1024: 
                    size = BasicImageSizes.ImageSize_1024_1024;
                    break;
                case ImageSelectionSize.x512x512:
                    size = BasicImageSizes.ImageSize_512_512;
                    break;
                case ImageSelectionSize.x256x256:
                    size = BasicImageSizes.ImageSize_256_256;
                    break;
            }
            
            var request = new ImageOpenAIRequest(APIKey, new ImageParameters () {
                prompt = Prompt,
                size = size,
                n = NumberOfImages,
            });

            request.SendRequest(action);
        }
    }
}