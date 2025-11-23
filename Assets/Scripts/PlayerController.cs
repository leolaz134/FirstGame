using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private Vector3 mouseScreenPosition; //Vector of where mouse is on screen in relation to camera
    private float xBounds = 25;
    private float zBounds = 25;
    public float speed;
    public float fireRateInterval;
    public float damage = 1;
    private float currentFireRateTime;
    public GameObject projectilePrefab;
    public static Transform instance; //Curent player physcial space/location for other parts of the program
    public bool gameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //InvokeRepeating("ProjectileSpawn", 1, fireRate);
        instance = transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentFireRateTime += Time.deltaTime;
        //Player loses control of everything at game over
        if (!gameOver)
        {
            MovePlayer();
            LookAtCursor();
            ConstrainPlayerPosition();
            GameOverCheck();
            if(currentFireRateTime >= fireRateInterval)
            {
                ProjectileSpawn();
                currentFireRateTime = 0;
            }
        }
        
    }

    public void ProjectileSpawn()
    {
        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }

    //Move player based on inputs
    void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed, Space.World);
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed, Space.World);
    }

    //Constrains player position to set bounds
    void ConstrainPlayerPosition()
    {


        if (transform.position.x < -xBounds)
        {
            transform.position = new Vector3(-xBounds, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xBounds)
        {
            transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
        }

        if (transform.position.z < -zBounds)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBounds);
        }

        if(transform.position.z > zBounds)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBounds);
        }
    }

    //Turns player to where the cursor is facing
    //-------
    //NOTE: these mouse position and moving to rotate to the mouse only works at 90 degree top down angle
    //If it's set up any other way I must use Rays
    void LookAtCursor()
    {
        

        //Get current mouse position on screen pixels
        mouseScreenPosition = Input.mousePosition;

        //Give Unity info and how far mouse is from camera in order to get correct position
        mouseScreenPosition.z = Camera.main.transform.position.y - transform.position.y;

        //convert mouse position to world position
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        //Get the direction of mouse from the player (Ignoring the height)
        //NOTE: if we input mouseWorldPos raw, our rotation will adjust looking at the
        //Camera position from 0,0,0 rather than from the player
        Vector3 direction = mouseWorldPos - transform.position;
        direction.y = 0f;


        transform.rotation = Quaternion.LookRotation(direction);
    }

    //Checks to see if player health goes below 0, thus killing the player
    void GameOverCheck()
    {
        if (transform.GetComponent<HealthBar>().currentHealth <= 0)
        {
            gameOver = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //If player gets hit by enemy, lose health
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.GetComponent<HealthBar>().loseHealth(1);
        }
    }

    //Behaviors for when player collides with powerups
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            PowerUpType powType = other.gameObject.GetComponent<Powerup>().powerUpType;
            Destroy(other.gameObject);
            //Note it will start spewing bullets when you reach a certain point
            if(powType == PowerUpType.FireRateBoost)
            {
                fireRateInterval -= .1f;
            } else if (powType == PowerUpType.DamageBoost)
            {
                damage++;
            } else if(powType == PowerUpType.Grenade)
            {
                //TODO: Implement grenade throwing behavior and object
            }
        }
    }
}
