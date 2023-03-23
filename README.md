# AIAssetGeneration

This project allows you to leverage [OpenAI's API](https://platform.openai.com/overview) to generate content for Unity. 

## Features
1. Generate Scripts, Shaders, and Textures for rapid prototyping, right in editor.
2. Open Source and fully extensible under MIT license.
3. Customizable to your workflows - leverage chatGPT to generate content for your game on your terms.


## Usage

1. Download the contents of Packages/com.recursiverhapsody.aissetgeneration into your project
2. Open your Project Settings and add your OpenAI key under "AI Generation". If you do not have an OpenAI key, head over to https://platform.openai.com/overview to generate one. NOTE: This key should be kept private, and will be stored under `UserSettings/`, so ensure that the key file is under your .gitignore.
3. Within your project, right click and hit Create > RecursiveRhapsody and create a `Generator` scriptable object. If you would like to generate images, try Advanced > MultiImageGeneratorSettings. If you want to generate scripts, try Basic > BasicScriptGeneratorSettings. 
4. Once you have created your generator, enter your query and setup your output. Your output will depend on your type of generator, but it will usually be either an exisiting asset, or an output directory.
5. These assets are designed to capture a "query" to chatGPT. It is recommended that you have different "Generators" for different assets - so if I have a brick wall background, I would create a BasicImageGeneratorSettings for my brick wall material. That way, I can modify or re-run my query for generating that texture in the future. 


Example (NOTE):

NOTE: Almost everything is this sample scene is AI generted. While the "SelfCLone" script is AI generated, I did make minor tweaks to it to prevent it from spawning a massive number of cloned objects.


![output4](https://user-images.githubusercontent.com/7245174/227133884-5d36695e-41c8-4115-af72-bbcf3efefb48.gif)
