

using UnityEngine;
 
public class SpinObject : MonoBehaviour
{
    public float rotationSpeed = 10f;
 
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}