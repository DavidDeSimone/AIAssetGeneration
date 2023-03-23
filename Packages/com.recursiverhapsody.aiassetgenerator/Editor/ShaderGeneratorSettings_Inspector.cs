using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace com.recursiverhapsody
{
    [CustomEditor(typeof(ShaderGeneratorSettings))]
    public class ShaderGeneratorSettings_Inspector : Editor
    {

        public VisualTreeAsset m_InspectorXML;
        private VisualElement inspector;

        public override VisualElement CreateInspectorGUI()
        {
            inspector = new VisualElement();
            m_InspectorXML.CloneTree(inspector);

            var inspectorFoldout = inspector.Q("Default_Inspector");
            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

            var button = inspector.Query("Generate_Button").First() as Button;
            button.clicked += OnGenerateClicked;

            return inspector;
        }

        public void OnGenerateClicked()
        {
            var ro = serializedObject.targetObject as ShaderGeneratorSettings;
            ro.SendRequest(delegate(ChatResponse result) {
                var resultText = result.choices[0].message.content;
                var label = inspector.Q("Result_Box") as TextField;
                label.value = resultText;

                File.WriteAllText(AssetDatabase.GetAssetPath(ro.ResultAsset), resultText);
                EditorUtility.SetDirty(ro.ResultAsset);
            });
        }
    }

}