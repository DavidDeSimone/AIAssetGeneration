using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace com.recursiverhapsody
{
    [CustomEditor(typeof(BasicImageVariationGeneratorSettings))]
    public class BasicImageVariationGeneratorSettings_Inspector : BaseInspector
    {
        public override void OnGenerateClicked()
        {
            var ro = serializedObject.targetObject as BasicImageVariationGeneratorSettings;
            ro.SendRequest(delegate(ImageResponse result) {
                for (var i = 0; i < result.data.Count; ++i)
                {
                    BaseOpenAIRequest<Texture2D>.RequestImage(result.data[i].url, delegate(Texture2D tex) {
                        var resultWindow = inspector.Q("Result_Image") as VisualElement;
                        resultWindow.style.backgroundImage = tex;

                        if (ro.OutputDirectory != default)
                        {
                            var guid = System.Guid.NewGuid();
                            File.WriteAllBytes(AssetDatabase.GetAssetPath(ro.OutputDirectory) + $"/{guid}.png", tex.EncodeToPNG());
                            EditorUtility.SetDirty(ro.OutputDirectory);
                        }

                        if (ro.ResultAsset != default && i == 0)
                        {
                            File.WriteAllBytes(AssetDatabase.GetAssetPath(ro.ResultAsset), tex.EncodeToPNG());
                            EditorUtility.SetDirty(ro.ResultAsset);
                        }

                        AssetDatabase.Refresh();
                        loadingInProgress = false;
                    });
                }
            }, OnError);
        }
    }

}