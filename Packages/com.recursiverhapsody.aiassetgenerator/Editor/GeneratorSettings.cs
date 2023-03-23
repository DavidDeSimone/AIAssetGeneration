using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace com.recursiverhapsody
{

    public interface IGeneratorSettings<T>
    where T: class
    {
        public void SendRequest(Action<T> action = null);
    }

    public class BaseGeneratorSettings : ScriptableObject
    {
        public TextAsset APIKeyPath;
    }
}