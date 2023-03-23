using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{

    [CreateAssetMenu(menuName = "RecursiveRhapsody/BasicScriptGeneratorSettings")]
    public class BasicScriptGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ChatResponse>
    {
        public TextAsset ResultAsset;
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
                        content = $"Generate a Unity C# script using the following prompt. Only respond with the script, do not provide any additional explanation. The first generated monobehavior should have the name {ResultAsset.name}. The prompt is: {Prompt}",
                    }
                }
            });

            request.SendRequest(action);
        }
    }
}