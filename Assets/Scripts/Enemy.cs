using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public int damageTaken = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //gets current instance of player and moves towards their position while look at player
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.position, speed * Time.deltaTime);
        transform.LookAt(PlayerController.instance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        //If enemy gets hit by projectile, lose health
        if (collision.gameObject.CompareTag("Projectile"))
        {
            transform.GetComponent<HealthBar>().loseHealth(damageTaken);
            Destroy(collision.gameObject);
        }

        
    }
}
