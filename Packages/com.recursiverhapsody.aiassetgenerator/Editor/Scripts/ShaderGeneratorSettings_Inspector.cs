using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace com.recursiverhapsody
{
    [CustomEditor(typeof(ShaderGeneratorSettings))]
    public class ShaderGeneratorSettings_Inspector : BaseInspector
    {
        public override void OnGenerateClicked()
        {
            var ro = serializedObject.targetObject as ShaderGeneratorSettings;
            ro.SendRequest(delegate(ChatResponse result) {
                var resultText = result.choices[0].message.content;
                var label = inspector.Q("Result_Box") as TextField;
                label.value = resultText;

                File.WriteAllText(AssetDatabase.GetAssetPath(ro.ResultAsset), resultText);
                EditorUtility.SetDirty(ro.ResultAsset);
                AssetDatabase.Refresh();
                loadingInProgress = false;
            }, OnError);
        }
    }
}