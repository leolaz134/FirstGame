using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 10, 0);
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    //TODO: This script poops itself when the player is destroyed, prob use game over bool or smthin
    void LateUpdate()
    {
     
            transform.position = player.transform.position + offset;
        
    }
}
