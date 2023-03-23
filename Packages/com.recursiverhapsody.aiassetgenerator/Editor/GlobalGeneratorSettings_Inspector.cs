using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;


namespace com.recursiverhapsody
{
    [CustomEditor(typeof(GlobalGeneratorSettings))]
    public class GlobalGeneratorSettings_Inspector : Editor
    {

        public VisualTreeAsset m_InspectorXML;

        public override VisualElement CreateInspectorGUI()
        {
            var inspector = new VisualElement();
            m_InspectorXML.CloneTree(inspector);

            var inspectorFoldout = inspector.Q("Default_Inspector");
            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

            var button = inspector.Query("Generate_Button").First() as Button;
            button.clicked += OnGenerateClicked;

            return inspector;
        }

        public void OnGenerateClicked()
        {
            Debug.Log("On Click..." + serializedObject.FindProperty("Prompt").stringValue);
            var apiKeyAsset = serializedObject.FindProperty("APIKeyPath").objectReferenceValue as TextAsset;

            var request = new ChatOpenAIRequest(apiKeyAsset.text);
            request.Request().Wait();
        }
    }

}