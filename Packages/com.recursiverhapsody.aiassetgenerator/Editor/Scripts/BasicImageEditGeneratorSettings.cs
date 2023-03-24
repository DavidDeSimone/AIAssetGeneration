using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    [CreateAssetMenu(menuName = "RecursiveRhapsody/Basic/ImageEditGeneratorSettings")]
    public class BasicImageEditGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ImageResponse>
    {
        public Texture2D ReferenceAsset;
        public Texture2D MaskAsset;
        public Texture2D ResultAsset;
        public DefaultAsset OutputDirectory;
        [Range(1,4)]
        public int NumberOfImages = 1;
        [TextArea(3, 10)]
        public string Prompt;

        public void SendRequest(Action<ImageResponse> action = null, Action<string> error = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var request = new ImageEditOpenAIRequest(APIKey, new ImageEditParameters () {
                image = ReferenceAsset,
                mask = MaskAsset,
                prompt = Prompt,
                n = NumberOfImages,
            });

            request.SendRequest(action, error);
        }
    }
}