using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    [CreateAssetMenu(menuName = "RecursiveRhapsody/BasicImageEditGeneratorSettings")]
    public class BasicImageEditGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ImageResponse>
    {
        public Texture2D ReferenceAsset;
        public Texture2D ResultAsset;
        [TextArea(3, 10)]
        public string Prompt;

        public void SendRequest(Action<ImageResponse> action = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var request = new ImageEditOpenAIRequest(APIKey, new ImageEditParameters () {
                image = ReferenceAsset,
                prompt = Prompt,
            });

            request.SendRequest(action);
        }
    }
}