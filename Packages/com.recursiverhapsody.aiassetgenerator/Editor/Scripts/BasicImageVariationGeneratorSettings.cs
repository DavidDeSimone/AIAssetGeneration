using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    [CreateAssetMenu(menuName = "RecursiveRhapsody/BasicImageVariationGeneratorSettings")]
    public class BasicImageVariationGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ImageResponse>
    {
        public Texture2D ReferenceAsset;
        public Texture2D ResultAsset;

        public void SendRequest(Action<ImageResponse> action = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var request = new ImageVariationOpenAIRequest(APIKey, new ImageVariationParameters () {
                image = ReferenceAsset,
            });

            request.SendRequest(action);
        }
    }
}