using UnityEngine;

public class SpinningBladesBehavior : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //If blade hits enemy, enemy loses health
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            enemy.GetComponent<HealthBar>().loseHealth(other.gameObject.GetComponent<Enemy>().damageTaken);
        }
    }
}
