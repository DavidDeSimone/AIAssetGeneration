using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace com.recursiverhapsody
{
    [CustomEditor(typeof(MultiImageGeneratorSettings))]
    public class MultiImageGeneratorSettings_Inspector : BaseInspector
    {

        private void ResetImages()
        {
            for (var i = 0; i < 4; ++i)
            {
                var resultWindow = inspector.Q($"Result_Image_{i + 1}") as VisualElement;
                resultWindow.style.backgroundImage = null;
            }
        }

        private void ProcessImage(MultiImageGeneratorSettings ro, string url, int idx)
        {
            BaseOpenAIRequest<Texture2D>.RequestImage(url, delegate(Texture2D tex) {
                var resultWindow = inspector.Q($"Result_Image_{idx + 1}") as VisualElement;
                resultWindow.style.backgroundImage = tex;

                var guid = System.Guid.NewGuid();
                File.WriteAllBytes(AssetDatabase.GetAssetPath(ro.OutputDirectory) + $"/{guid}.png", tex.EncodeToPNG());
                EditorUtility.SetDirty(ro.OutputDirectory);
                AssetDatabase.Refresh();
                loadingInProgress = false;
            });
        }

        public override void OnGenerateClicked()
        {
            var ro = serializedObject.targetObject as MultiImageGeneratorSettings;
            ro.SendRequest(delegate(ImageResponse result) {
                ResetImages();
                for (var i = 0; i < result.data.Count; ++i)
                {
                    ProcessImage(ro, result.data[i].url, i);
                }
            }, OnError);
        }
    }

}