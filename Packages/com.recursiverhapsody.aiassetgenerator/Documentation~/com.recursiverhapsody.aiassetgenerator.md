# Quick Start

1. \> [Download](https://github.com/DavidDeSimone/AIAssetGeneration) the contents of Packages/com.recursiverhapsody.aissetgeneration into your project
2. \> Open your Project Settings and add your OpenAI key under "AI Generation". If you do not have an OpenAI key, head over to https://platform.openai.com/overview to generate one. NOTE: This key should be kept private, and will be stored under `UserSettings/`, so ensure that the key file is under your .gitignore.
3. \> Within your project, right click and hit Create > RecursiveRhapsody > Advanced > MultiImageGeneratorSettings. 
4. \> Select your MultiImageGeneratorSettings asset and select your OutputDirectory
5. \> Input your prompt into the prompt field
6. \> Hit Generate! 

# Generator Types

## BasicTextGenerator

The purpose of the BasicTextGenerator is to generate text output from [one of the ChatGPT models.](https://platform.openai.com/docs/models/overview). Note that as of the writing of this documentation, GPT-4 is in a closed beta, and you may not have access to it. Everyone should have access to GPT-3. This will utilize the [OpenAI chat endpoint](https://platform.openai.com/docs/api-reference/chat) to generate a response. This response will be shown in editor, and written to the specified output file. Note that this will OVERWRITE the content of the file that you specify.

The primary usage will be to describe what you would like to GPT and press "Generate". This can range from a request to generate NPC dialog, to generate an idea's document, or formatted texts like JSON or YML.


While you can use this to generator to build shaders, C# scripts, we offer specialized generators with more tailored queries to generate better results for those type of assets. 

## BasicScriptGenerator

The purpose of the BasicScriptGenerator is to generate a C# script. Once this script is generated, we will perform an editor Refresh(). ResultAsset should be a C# script asset. NOTE: This will OVERWRITE the content of the ResultAsset. We add a prefix to your prompt that reads:

``` c#
messages = new List<Message>() {
    new Message() {
        role = Roles.User,
        content = "Generate a Unity C# script using the following prompt." 
        + "Only respond with the script, do not provide any additional explanation." 
        + "If you generate a monobehavior, the first generated monobehavior should have " 
        + $"the name {ResultAsset.name}. The prompt is: {Prompt}",
    }
};
```

## BasicImageGenerator

The purpose of BasicImageGenerator is to generate an image. The most common use case will be 2D textures, background or concept art. This can generate images either in-place (by specifying ResultAsset), or generate an image and writing that image to a directory (by specifying OutputDirectory). You can specify BOTH ResultAsset and OutputDirectory. If you specify OutputDirectory, this generator will name the image(s) `<new guid>.png`

You can choose the size of the result image - options are 1024x1024, 512x512 or 256x256.

## BasicImageVariation

The purpose of BasicImageVariation is to generate a variation of an exisiting image. This will likely be useful if you have art of images that you want to "tweak" or generate multiple random variations of. Like BasicImageGenerator, you can specify a ResultAsset, an OutputDirectory, or both. If you specify OutputDirectory, this generator will name the image(s) `<new guid>.png`

You can choose the size of the result image - options are 1024x1024, 512x512 or 256x256

You can generate between 1 and 4 image variations per usage. If you specify a ResultAsset, the FIRST generated image will be written to ResultAsset.

Note: This generator does not take a prompt. 

## BasicImageEdit

The purpose of BasicImageEdit is to edit an existing image. There are [examples of OpenAI's website](https://platform.openai.com/docs/api-reference/images/create-edit). You can either provide a transparency mask (optional), or have transparency within your image. Edits to this image will be limited to this area of transparency.

You can choose the size of the result image - options are 1024x1024, 512x512 or 256x256

You can generate between 1 and 4 image variations per usage. If you specify a ResultAsset, the FIRST generated image will be written to ResultAsset.

## MultiImageGenerator

The purpose of MultiImageGenerator is to generate multiple new images. These images will be output to OutputDirectory, and named  `<new guid>.png`

## ShaderGenerator

The purpose of ShaderGenerator is to generate shader assets. These images will be output to OutputDirectory, and named `<new guid>.shader`

To generate the shader, we add the following prefix:

``` c#
messages = new List<Message>() {
    new Message() {
        role = Roles.User,
        content = $"Generate a Unity shader using the following prompt, " 
        + $"only respond with the unity shader. {Prompt}",
    }
}
```



