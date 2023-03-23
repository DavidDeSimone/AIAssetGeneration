using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{

    [CreateAssetMenu(menuName = "RecursiveRhapsody/BasicTextGeneratorSettings")]
    public class BasicTextGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ChatResponse>
    {
        [TextArea(3, 10)]
        public string Prompt;

        public void SendRequest(Action<ChatResponse> action = null)
        {
            var request = new ChatOpenAIRequest(APIKeyPath.text, new ChatParameters () {
                model = ChatModel.ChatGPT_3_5,
                messages = new List<Message>() {
                    new Message() {
                        role = Roles.User,
                        content = Prompt,
                    }
                }
            });

            request.SendRequest(action);
        }
    }
}