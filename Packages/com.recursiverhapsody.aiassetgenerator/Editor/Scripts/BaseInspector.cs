using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace com.recursiverhapsody
{
    public abstract class BaseInspector : Editor
    {

        public VisualTreeAsset m_InspectorXML;
        protected VisualElement inspector;
        protected bool loadingInProgress;

        public override VisualElement CreateInspectorGUI()
        {
            inspector = new VisualElement();
            m_InspectorXML.CloneTree(inspector);

            var inspectorFoldout = inspector.Q("Default_Inspector");
            InspectorElement.FillDefaultInspector(inspectorFoldout, serializedObject, this);

            var button = inspector.Query("Generate_Button").First() as Button;
            button.clicked += GenerateClicked;

            return inspector;
        }

        private IEnumerator<string> AdvanceLoadingBar()
        {
            var loadingBar = inspector.Q("Request_Progress") as ProgressBar;
            var interval = 0.5f;
            var lastTime = Time.realtimeSinceStartup;
            while (loadingInProgress)
            {
                if (Time.realtimeSinceStartup - lastTime > interval)
                {
                    lastTime = Time.realtimeSinceStartup;
                    loadingBar.value = (loadingBar.value + 1) % 100;
                }

                yield return null;
            } 

            loadingBar.style.display = DisplayStyle.None;
            yield return "success";
        }

        public void GenerateClicked()
        {
            var loadingBar = inspector.Q("Request_Progress") as ProgressBar;
            loadingBar.style.display = DisplayStyle.Flex;
            OnGenerateClicked();

            loadingInProgress = true;
            BaseOpenAIRequest<ChatResponse>.StartBackgroundTask<string>(AdvanceLoadingBar());
        }

        public abstract void OnGenerateClicked();
    }

}