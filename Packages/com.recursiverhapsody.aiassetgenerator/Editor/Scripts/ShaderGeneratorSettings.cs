using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace com.recursiverhapsody
{

    [CreateAssetMenu(menuName = "RecursiveRhapsody/Basic/ShaderGeneratorSettings")]
    public class ShaderGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ChatResponse>
    {
        public CompletionModels Model;
        public Shader ResultAsset;
        public DefaultAsset OutputDirectory;

        [TextArea(3, 10)]
        public string Prompt;

        public void SendRequest(Action<ChatResponse> action = null, Action<string> error = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var model = CompletionModelNames.MapEnumToName(Model);
            var request = new ChatOpenAIRequest(APIKey, new ChatParameters () {
                model = model,
                messages = new List<Message>() {
                    new Message() {
                        role = Roles.User,
                        content = $"Generate a Unity shader using the following prompt, only respond with the unity shader. {Prompt}",
                    }
                }
            });

            request.SendRequest(action, error);
        }
    }
}