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
            if (m_InspectorXML == null) {
                return new VisualElement();
            }

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
            loadingBar.value = 1;
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

        protected void OnError(string error)
        {
            var loadingBar = inspector.Q("Request_Progress") as ProgressBar;
            loadingBar.style.display = DisplayStyle.None;
        }

        public void GenerateClicked()
        {
            var loadingBar = inspector.Q("Request_Progress") as ProgressBar;
            loadingBar.style.display = DisplayStyle.Flex;
            OnGenerateClicked();

            loadingInProgress = true;
            BaseOpenAIRequest<ChatResponse>.StartBackgroundTask<string>(AdvanceLoadingBar(), null, OnError);
        }

        public abstract void OnGenerateClicked();
    }
}