using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    public GameObject bladePivot;
    public GameObject titleScreen;
    public new GameObject camera;
    public List<GameObject> powerups;
    private float spawnInterval = 3f; //Interval each of seconds each enemy spawns
    private float currentSpawnTime = 0; //current time in reference to spawn time
    private float bigCountdown = 10; //time amount needed to increase spawn time
    private float currentBigTime = 0; //current time in reference to big countdown
    private float difficultyIncreaseTime = 20;//time amount to increase enemy health and speed
    private float currentDifficultyTime = 0;//current time in reference to health increase time
    private int healthIncreaseAmount = 0; //ampount of health to increase each interval
    private float speedIncreaseAmount = 0; //amount of speed to increase each interval
    private float xBounds = 25;
    private float zBounds = 25;
    private bool gameStarted = false;


    //TODO: Make a restart screen when you die
    //use SceneManager.LoadScene(SceneManager.GetActiveScene().name); for restart button functionality

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("PowerupSpawnRandomPosition", 3, 3);
        StartCoroutine("TimeCount");
    }

    // Update is called once per frame
    void Update()
    {
        //Start game logic when play button is started
        if (gameStarted)
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
                healthIncreaseAmount++;
                speedIncreaseAmount++;
                currentDifficultyTime = 0;
            }
        }
    }

    //Spawns Enemies prefab at a random positon until game is over
    void EnemySpawnRandomPosition()
    {
        if (player.GetComponent<PlayerController>().gameOver)
        {
            return;
        }
        int maxAttempts = 10;
        int minSpawnDistance = 5;
        //setting current spawn distance to the minimum distance initially
        Vector3 currentSpawnDistance = player.transform.position + new Vector3(minSpawnDistance, 0, 0);

        //Within max attempts, try to find a valid spawn position
        //otherwise spawn it in that close range
        for (int i = 0; i < maxAttempts; i++)
        {
            float randX = Random.Range(-xBounds, xBounds);
            float randZ = Random.Range(-zBounds, zBounds);
            Vector3 randPos = new Vector3(randX, enemyPrefab.transform.position.y, randZ);

            if (Vector3.Distance(randPos, player.transform.position) >= minSpawnDistance)
            {
                currentSpawnDistance = randPos;
                break;
            }
        }
        
        //Must change varibales of an instance of an enemy in order to avoid changing the prefab itself
        GameObject enemyInstance = Instantiate(enemyPrefab, currentSpawnDistance, transform.rotation);
        enemyInstance.GetComponent<HealthBar>().maxHealth += healthIncreaseAmount;
        enemyInstance.GetComponent<Enemy>().speed += speedIncreaseAmount;
    }

    //Spawn a Powerup prefab randomly
    void PowerupSpawnRandomPosition()
    {
        if (!player.GetComponent<PlayerController>().gameOver)
        {
            int randIndex = Random.Range(0, powerups.Count);
            GameObject powerup = powerups[randIndex];
            float randX = Random.Range(-xBounds, xBounds);
            float randZ = Random.Range(-zBounds, zBounds);
            Vector3 randPos = new Vector3(randX, powerup.transform.position.y, randZ);
            Instantiate(powerup, randPos, transform.rotation);
        }
    }

    //Method to start the game when play button is pressed
    public void StartGame()
    {
        gameStarted = true;
        Instantiate(player, player.transform.position, Quaternion.identity);
        titleScreen.SetActive(false);
        camera.GetComponent<CameraFollowPlayer>().target = GameObject.FindWithTag("Player");
    }

    //Standard time count for the game, recording every second
    //TODO: Maybe show time UI element in top left corner later, and see how long it takes for
    //spawns to go crazy and call the game that
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
