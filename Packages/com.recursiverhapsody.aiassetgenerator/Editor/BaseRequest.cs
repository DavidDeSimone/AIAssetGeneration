using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace com.recursiverhapsody
{
    public enum ChatModels
    {
        ChatGPT_3_5,
        Davinci_3,
    }


    public interface IOpenAIRequest {
        public Task Request();
    }
    public abstract class BaseOpenAIRequest : IOpenAIRequest
    {
        protected string apiKey;
        public BaseOpenAIRequest(string apiKey)
        {
            this.apiKey = apiKey;
            Debug.Log("Created with APIKEY " + apiKey);
        }


        public async Task Request()
        {
            Debug.Log("Requesting.....");
        } 
    }

    public class ChatOpenAIRequest : BaseOpenAIRequest 
    {
        public ChatOpenAIRequest(string apiKey) : base(apiKey) {}
    }
}