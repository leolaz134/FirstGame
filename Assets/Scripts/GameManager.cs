using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<GameObject> powerups;
    public GameObject playerObject;
    private float spawnInterval = 3f; //Interval each of seconds each enemy spawns
    private float currentSpawnTime = 0; //current time in reference to spawn time
    private float bigCountdown = 30; //time amount needed to increase spawn time
    private float currentBigTime = 0; //current time in reference to big countdown
    private float healthIncreaseTime = 60;//time amount to increase enemy health
    private float currentHealthTime = 0;//current time in reference to health increase time
    private float xBounds = 25;
    private float zBounds = 25;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //maybe make a spawning a coroutine, and also increase health with spawining
    //maybe add more enemies?
    void Start()
    {
        InvokeRepeating("PowerupSpawnRandomPosition", 3, 3);
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentBigTime += Time.deltaTime;

        //this section spawns an enemy for the interval
        if (currentSpawnTime >= spawnInterval)
        {
            EnemySpawnRandomPosition();
            currentSpawnTime = 0;
        }

        //this section reduces the interval to spawn an enemy
        if (currentBigTime >= bigCountdown)
        {
            spawnInterval -= .25f;
            currentBigTime = 0;
        }
        
        //this section increases enemy health after a certain amount of time
        if (currentHealthTime >= healthIncreaseTime)
        {
            enemyPrefab.GetComponent<HealthBar>().maxHealth++;
            currentHealthTime = 0;
        }
    }

    //Spawns a Powerup prefab at a random positon until game is over
    void EnemySpawnRandomPosition()
    {
        if (!playerObject.GetComponent<PlayerController>().gameOver)
        {
            float randX = Random.Range(-xBounds, xBounds);
            float randZ = Random.Range(-zBounds, zBounds);
            //Redo ranges if enemy spawn is too close to player, sometimes it's wonky and heavily delays spawns
            if (randX < playerObject.transform.position.x + 5 && randX > playerObject.transform.position.x - 5 || 
            randZ < playerObject.transform.position.z + 5 && randX > playerObject.transform.position.z - 5)
            {
                EnemySpawnRandomPosition();
            }
            else
            {
                Vector3 randPos = new Vector3(randX, enemyPrefab.transform.position.y, randZ);
                Instantiate(enemyPrefab, randPos, transform.rotation);
            }
            
        }
    }

    //Spawn a Powerup randomly
    void PowerupSpawnRandomPosition()
    {
        if (!playerObject.GetComponent<PlayerController>().gameOver)
        {
            int randIndex = Random.Range(0,powerups.Count);
            GameObject powerup = powerups[randIndex];
            float randX = Random.Range(-xBounds, xBounds);
            float randZ = Random.Range(-zBounds, zBounds);
            Vector3 randPos = new Vector3(randX, powerup.transform.position.y, randZ);
            Instantiate(powerup, randPos, transform.rotation);
        }
    }

}
