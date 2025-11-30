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
    private float bigCountdown = 10; //time amount needed to increase spawn time
    private float currentBigTime = 0; //current time in reference to big countdown
    private float difficultyIncreaseTime = 20;//time amount to increase enemy health and speed
    private float currentDifficultyTime = 0;//current time in reference to health increase time
    private float xBounds = 25;
    private float zBounds = 25;

    public int startingMaxHealth = 2;
    public int startingMaxSpeed = 3;

    //TODO: Make the game work with the UI, such as spawning everything in when play is pressed
    //and having a game over screen with a restart button

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("PowerupSpawnRandomPosition", 3, 3);
        StartCoroutine("TimeCount");
        //Since I modify prefabs I must set the orignanl values back here
        //Maybe there's an easier way to do this?
        enemyPrefab.GetComponent<HealthBar>().maxHealth = startingMaxHealth;
        enemyPrefab.GetComponent<Enemy>().speed = startingMaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentBigTime += Time.deltaTime;
        currentDifficultyTime += Time.deltaTime;

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
        if (currentDifficultyTime >= difficultyIncreaseTime)
        {
            enemyPrefab.GetComponent<HealthBar>().maxHealth++;
            enemyPrefab.GetComponent<Enemy>().speed++;
            currentDifficultyTime = 0;
        }
    }

    //Spawns a Powerup prefab at a random positon until game is over
    //TODO: Either make a better spawning random position or increase spawn rate increase overtime
    void EnemySpawnRandomPosition()
    {
        if (playerObject.GetComponent<PlayerController>().gameOver)
        {
            return;
        }
        int maxAttempts = 10;
        int minSpawnDistance = 5;

        //Within max attempts, try to find a valid spawn position
        //otherwise spawn it in that close range
        for (int i = 0; i < maxAttempts; i++)
        {
            float randX = Random.Range(-xBounds, xBounds);
            float randZ = Random.Range(-zBounds, zBounds);
            Vector3 randPos = new Vector3(randX, enemyPrefab.transform.position.y, randZ);

            if (Vector3.Distance(randPos, playerObject.transform.position) >= minSpawnDistance)
            {
                Instantiate(enemyPrefab, randPos, transform.rotation);
                return;
            }
        }
        //Just defaults to spawning it 5 units away on the x axis
        Instantiate(enemyPrefab, playerObject.transform.position + new Vector3(minSpawnDistance, 0, 0), transform.rotation);
    }

    //Spawn a Powerup randomly
    void PowerupSpawnRandomPosition()
    {
        if (!playerObject.GetComponent<PlayerController>().gameOver)
        {
            int randIndex = Random.Range(0, powerups.Count);
            GameObject powerup = powerups[randIndex];
            float randX = Random.Range(-xBounds, xBounds);
            float randZ = Random.Range(-zBounds, zBounds);
            Vector3 randPos = new Vector3(randX, powerup.transform.position.y, randZ);
            Instantiate(powerup, randPos, transform.rotation);
        }
    }

    //Standard time count for the game, recording every second
    IEnumerator TimeCount()
    {
        int time = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log(++time);
        }
    }

}
