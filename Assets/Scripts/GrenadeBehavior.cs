using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{
    public float explosionForce;
    public float explosionRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("Grenade");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Grenade()
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        yield return new WaitForSeconds(3);

        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position. And have enemies take damage from our damage stat
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
                enemies[i].GetComponent<HealthBar>().loseHealth(enemies[i].GetComponent<Enemy>().damageTaken);
            }
        }

        Destroy(gameObject);
        
    }
}
