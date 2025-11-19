using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float xBounds = 25;
    private float zBounds = 25;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -xBounds || transform.position.x > xBounds
            || transform.position.z < -zBounds || transform.position.z > zBounds)
        {
            Destroy(gameObject);
        }
    }
}
