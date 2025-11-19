using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public PlayerController player;
    private float spawnInterval = 3f; //Interval each of seconds each enemy spawns
    private float currentSpawnTime = 0; //current time in reference to spawn time
    private float bigCountdown = 30; //time amount needed to increase spawn time
    private float currentBigTime = 0; //current time in reference to big countdown
    private float healthIncreaseTime = 60;//time amount to increase enemy health
    private float currentHealthTime = 0;//current time in reference to health increase time
    private float xBounds = 25;
    private float zBounds = 25;


    //TODO: Increase (or should I say decrease spawn rate) value over time
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //maybe make a spawning a coroutine, and also increase health with spawining
    void Start()
    {

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

    //Spawns a Enemy prefab at a random positon until game is over
    //TODO: Make it so it will always spawn within a certain range of the player
    void EnemySpawnRandomPosition()
    {
        if (!player.gameOver)
        {
            float randX = Random.Range(-xBounds, xBounds);
            float randZ = Random.Range(-zBounds, zBounds);
            Vector3 randPos = new Vector3(randX, enemyPrefab.transform.position.y, randZ);
            Instantiate(enemyPrefab, randPos, transform.rotation);
        }
    }


}
