using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.recursiverhapsody
{

    // [CreateAssetMenu(menuName = "RecursiveRhapsody/GlobalGeneratorSettings")]
    public class BaseGeneratorSettings : ScriptableObject
    {
        public TextAsset APIKeyPath;
        // [TextArea(3, 10)]
        // public string Prompt;
    }
}