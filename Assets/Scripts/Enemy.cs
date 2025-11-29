using UnityEngine;

public class Enemy : MonoBehaviour
{
    //For now this script I'll use the player instance and find with tag just to get practice with it
    //but this will probably be easier if I just have a public reference
    //Also the way I do health is weird, next prob have loss of health happen in ability to different damage instances
    public float speed;
    public float damageTaken;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set damage taken to be player damage
        //Probably would've been better to implement damage to projectile so it can work both player and enemy
        damageTaken = GameObject.FindWithTag("Player").GetComponent<PlayerController>().damage;  
    }

    // Update is called once per frame
    void Update()
    {
        //gets current instance of player and moves towards their position while look at player
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.position, speed * Time.deltaTime);
        transform.LookAt(PlayerController.instance);
        //Gotta have it here so it keeps updating
        damageTaken = GameObject.FindWithTag("Player").GetComponent<PlayerController>().damage;
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
