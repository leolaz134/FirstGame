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
        //It won't collide with the enmy prefab child obejcts because they are on a different physics layer
        //If a Trigger collider moves using TRANSFORM, it can ONLY fire trigger events on KINEMATIC rigidbodies
        //and it won't collide with non-kinematic rigidbodies

        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = other.gameObject;
            enemy.GetComponent<HealthBar>().loseHealth(other.gameObject.GetComponent<Enemy>().damageTaken);
            Debug.Log("Hit");
        }
    }
}
