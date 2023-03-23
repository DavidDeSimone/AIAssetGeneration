using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    // @TODO(DDS) Make a version that always dumps the image to a dir
    // that should be the default. Inplace should be specialized.
    [CreateAssetMenu(menuName = "RecursiveRhapsody/BasicImageGeneratorSettings")]
    public class BasicImageGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ImageResponse>
    {
        public Texture2D ResultAsset;
        [TextArea(3, 10)]
        public string Prompt;
        // @TODO
        // Used for always-add portion
        // public DefaultAsset Result;

        public void SendRequest(Action<ImageResponse> action = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var request = new ImageOpenAIRequest(APIKey, new ImageParameters () {
                prompt = Prompt,
            });

            request.SendRequest(action);
        }
    }
}