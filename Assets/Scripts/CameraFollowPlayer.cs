using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 10, 0);
    public GameObject target = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.transform.position + offset;
        }
        
        
    }
}
