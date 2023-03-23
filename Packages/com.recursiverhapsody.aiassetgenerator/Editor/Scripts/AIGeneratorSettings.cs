// Derived from https://github.com/keijiro/AICommand/blob/main/Assets/Editor/AIGeneratorSettings.cs
// under permissive unlicense license. Credit to keijiro

using UnityEngine;
using UnityEditor;

namespace com.recursiverhapsody {

[FilePath("UserSettings/AIGeneratorSettings.asset",
          FilePathAttribute.Location.ProjectFolder)]
    public sealed class AIGeneratorSettings : ScriptableSingleton<AIGeneratorSettings>
    {
        public string apiKey = null;
        public void Save() => Save(true);
        void OnDisable() => Save();
    }

    sealed class AIGeneratorSettingsProvider : SettingsProvider
    {
        public AIGeneratorSettingsProvider()
        : base("Project/AI Generator", SettingsScope.Project) {}

        public override void OnGUI(string search)
        {
            var settings = AIGeneratorSettings.instance;

            var key = settings.apiKey;
            EditorGUI.BeginChangeCheck();

            key = EditorGUILayout.TextField("API Key", key);

            if (EditorGUI.EndChangeCheck())
            {
                settings.apiKey = key;
                settings.Save();
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateCustomSettingsProvider()
        => new AIGeneratorSettingsProvider();
    }

}