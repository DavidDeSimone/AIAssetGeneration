using UnityEngine;

namespace com.recursiverhapsody
{
    public enum ChatModels
    {
        ChatGPT_3_5,
        Davinci_3,
    }


    public interface IOpenAIRequest {}
    public abstract class BaseOpenAIRequest : IOpenAIRequest
    {

    }
}