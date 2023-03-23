using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace com.recursiverhapsody
{
    [CustomEditor(typeof(BasicImageEditGeneratorSettings))]
    public class BasicImageEditGeneratorSettings_Inspector : Editor
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
            var ro = serializedObject.targetObject as BasicImageEditGeneratorSettings;
            ro.SendRequest(delegate(ImageResponse result) {
                BaseOpenAIRequest<Texture2D>.RequestImage(result.data[0].url, delegate(Texture2D tex) {
                    var resultWindow = inspector.Q("Result_Image") as VisualElement;
                    resultWindow.style.backgroundImage = tex;

                    File.WriteAllBytes(AssetDatabase.GetAssetPath(ro.ResultAsset), tex.EncodeToPNG());
                    EditorUtility.SetDirty(ro.ResultAsset);
                    AssetDatabase.Refresh();
                });
            });
        }
    }

}