using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.recursiverhapsody
{

    [CreateAssetMenu(menuName = "RecursiveRhapsody/BasicTextGeneratorSettings")]
    public class BasicTextGeneratorSettings : BaseGeneratorSettings
    {
        [TextArea(3, 10)]
        public string Prompt;
    }
}