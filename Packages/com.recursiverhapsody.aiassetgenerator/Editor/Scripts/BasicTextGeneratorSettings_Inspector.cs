using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;

namespace com.recursiverhapsody
{
    [CustomEditor(typeof(BasicTextGeneratorSettings))]
    public class BasicTextGeneratorSettings_Inspector : BaseInspector
    {

        public override void OnGenerateClicked()
        {
            var ro = serializedObject.targetObject as BasicTextGeneratorSettings;
            ro.SendRequest(delegate(ChatResponse result) {
                var resultText = result.choices[0].message.content;
                var label = inspector.Q("Result_Box") as TextField;
                label.value = resultText;
                label.style.display = DisplayStyle.Flex;

                File.WriteAllText(AssetDatabase.GetAssetPath(ro.ResultAsset), resultText);
                EditorUtility.SetDirty(ro.ResultAsset);
                loadingInProgress = false;
            });
        }
    }

}