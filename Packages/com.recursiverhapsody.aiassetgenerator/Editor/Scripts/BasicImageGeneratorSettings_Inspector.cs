using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace com.recursiverhapsody
{
    [CustomEditor(typeof(BasicImageGeneratorSettings))]
    public class BasicImageGeneratorSettings_Inspector : BaseInspector
    {
        public override void OnGenerateClicked()
        {
            var loadingBar = inspector.Q("Request_Progress") as ProgressBar;
            loadingBar.style.display = DisplayStyle.Flex;

            var ro = serializedObject.targetObject as BasicImageGeneratorSettings;
            ro.SendRequest(delegate(ImageResponse result) {
                BaseOpenAIRequest<Texture2D>.RequestImage(result.data[0].url, delegate(Texture2D tex) {
                    var resultWindow = inspector.Q("Result_Image") as VisualElement;
                    resultWindow.style.backgroundImage = tex;


                    if (ro.ResultAsset != default)
                    {
                        File.WriteAllBytes(AssetDatabase.GetAssetPath(ro.ResultAsset), tex.EncodeToPNG());
                        EditorUtility.SetDirty(ro.ResultAsset);
                    }

                    if (ro.OutputDirectory != default)
                    {
                        var guid = System.Guid.NewGuid();
                        File.WriteAllBytes(AssetDatabase.GetAssetPath(ro.OutputDirectory) + $"/{guid}.png", tex.EncodeToPNG());
                        EditorUtility.SetDirty(ro.OutputDirectory);
                    }

                    AssetDatabase.Refresh();
                    loadingInProgress = false;
                });
            }, OnError);
        }
    }

}