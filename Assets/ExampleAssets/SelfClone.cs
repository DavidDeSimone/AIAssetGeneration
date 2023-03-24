

using UnityEngine;

namespace com.recursiverhapsody.examples
{

public class SelfClone : MonoBehaviour
{
    [SerializeField] private GameObject childPrefab;
    [SerializeField] private float scale;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float distance;
    private int cloneCount;

    private void Start()
    {
        cloneCount = 0;
        CreateChild();
    }

    private void CreateChild()
    {
        while (cloneCount < 5 && childPrefab != null)
        {
            GameObject child = Instantiate(childPrefab, transform.position, Quaternion.identity);
            child.transform.localScale = transform.localScale * scale;
            child.transform.position = new Vector3(transform.position.x + (distance * cloneCount), transform.position.y, transform.position.z + (distance * cloneCount));
            child.transform.SetParent(transform);
            cloneCount++;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}

}