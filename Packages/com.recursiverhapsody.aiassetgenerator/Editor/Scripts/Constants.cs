
namespace com.recursiverhapsody
{
    public class ChatModel
    {
        public const string ChatGPT_3_5 = "gpt-3.5-turbo";
        public const string Davinci_3 = "text-davinci-003";
    }

    public class Roles
    {
        public const string User = "user";
        public const string System = "system";
        public const string Assistant = "assistant";
    }

    public class BasicImageSizes
    {
        public const string ImageSize_256_256 = "256x256";
        public const string ImageSize_512_512 = "512x512";
        public const string ImageSize_1024_1024 = "1024x1024";
    }

    public class ImageResponseFormat
    {
        public const string Url = "url";
        public const string B64_Json = "b64_json";
    }
}