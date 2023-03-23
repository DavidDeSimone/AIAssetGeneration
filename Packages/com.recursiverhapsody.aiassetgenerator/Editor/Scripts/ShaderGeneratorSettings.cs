using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace com.recursiverhapsody
{

    [CreateAssetMenu(menuName = "RecursiveRhapsody/ShaderGeneratorSettings")]
    public class ShaderGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ChatResponse>
    {
        public Shader ResultAsset;
        [TextArea(3, 10)]
        public string Prompt;

        public void SendRequest(Action<ChatResponse> action = null)
        {
            if (APIKey == null)
            {
                throw new NullReferenceException($"No API Key is set for {name}. Please set a valid OpenAI API Key. If you do not have one, you can get one at https://platform.openai.com/overview");
            }
            
            var request = new ChatOpenAIRequest(APIKey, new ChatParameters () {
                model = ChatModel.ChatGPT_3_5,
                messages = new List<Message>() {
                    new Message() {
                        role = Roles.User,
                        content = $"Generate a Unity shader using the following prompt, only respond with the unity shader. {Prompt}",
                    }
                }
            });

            request.SendRequest(action);
        }
    }
}