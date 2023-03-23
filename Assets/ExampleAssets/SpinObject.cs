

using UnityEngine;

public class SpinObject : MonoBehaviour
{
    private float scaleFactor = 0.01f;

    void Update()
    {
        transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}