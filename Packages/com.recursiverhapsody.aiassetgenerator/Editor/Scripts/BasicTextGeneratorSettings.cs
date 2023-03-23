using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace com.recursiverhapsody
{
    public class CompletionModelNames
    {
        public const string GPT_3_5_turbo = "gpt-3.5-turbo";
        public const string GPT_4 = "gpt-4";
        public const string GPT_4_0314 = "gpt-4-0314";
        public const string GPT_4_32k = "gpt-4-32k";
        public const string GPT_4_32k_0314 = " gpt-4-32k-0314";
        public const string GPT_3_5_turbo_0301 = "gpt-3.5-turbo-0301";

        public static string MapEnumToName(CompletionModels model)
        {
            switch (model)
            {
                case CompletionModels.GPT_3_5_turbo: return GPT_3_5_turbo;
                case CompletionModels.GPT_4: return GPT_4;
                case CompletionModels.GPT_4_0314: return GPT_4_0314;
                case CompletionModels.GPT_4_32k: return GPT_4_32k;
                case CompletionModels.GPT_4_32k_0314: return GPT_4_32k_0314;
                case CompletionModels.GPT_3_5_turbo_0301: return GPT_3_5_turbo_0301; 
            }

            throw new NotImplementedException($"Passed Unsupported Model {model} for chat completion.");
        }
    }
    public enum CompletionModels
    {
        [InspectorName("gpt-3.5-turbo")]
        GPT_3_5_turbo,
        [InspectorName("gpt-4 (Limited Beta - Check your OpenAI API plan)")]
        GPT_4,
        [InspectorName("gpt-4-0314 (Limited Beta - Check your OpenAI API plan)")]
        GPT_4_0314,
        [InspectorName("gpt-4-32k (Limited Beta - Check your OpenAI API plan)")]
        GPT_4_32k,
        [InspectorName("gpt-4-32k-0314 (Limited Beta - Check your OpenAI API plan)")]
        GPT_4_32k_0314,
        [InspectorName("gpt-3.5-turbo-0301")]
        GPT_3_5_turbo_0301,
    }

    [CreateAssetMenu(menuName = "RecursiveRhapsody/Basic/TextGeneratorSettings")]
    public class BasicTextGeneratorSettings : BaseGeneratorSettings, IGeneratorSettings<ChatResponse>
    {
        public CompletionModels Model;
        public TextAsset ResultAsset;
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
                        content = Prompt,
                    }
                }
            });

            request.SendRequest(action, error);
        }
    }
}