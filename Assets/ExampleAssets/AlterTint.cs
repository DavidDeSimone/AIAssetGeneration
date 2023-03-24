

using UnityEngine;

namespace com.recursiverhapsody.examples
{

public class AlterTint : MonoBehaviour
{
    private Renderer rend;
    private Color tintColor;
    private float t;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        tintColor = Color.blue;
        t = 0.0f;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t <= 1.0f)
        {
            tintColor = Color.Lerp(Color.blue, Color.red, t);
        }
        else
        {
            tintColor = Color.Lerp(Color.red, Color.blue, t - 1);
            if (t >= 2.0f)
            {
                t = 0.0f;
            }
        }

        rend.material.SetColor("_TintColor", tintColor);
    }
}

}