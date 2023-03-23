using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;


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
            var prompt = serializedObject.FindProperty("Prompt").stringValue;
            Debug.Log("On Click..." + prompt);
            var apiKeyAsset = serializedObject.FindProperty("APIKeyPath").objectReferenceValue as TextAsset;

            var request = new ChatOpenAIRequest(apiKeyAsset.text, new ChatOpenAIRequest.Parameters () {
                model = ChatModel.ChatGPT_3_5,
                messages = new List<ChatOpenAIRequest.Message>() {
                    new ChatOpenAIRequest.Message() {
                        role = Roles.User,
                        content = prompt,
                    }
                }
            });
            // EditorUtility.DisplayProgressBar("Simple Progress Bar", "Doing some work...", 0.5f);
            request.SendRequest();
            // EditorUtility.ClearProgressBar();
        }
    }

}