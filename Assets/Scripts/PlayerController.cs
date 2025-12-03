using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6;
    public float damage = 1;
    public float fireRateInterval = 1;
    public GameObject projectilePrefab;
    public GameObject grenade; //Grenade object to be thrown
    public float grenadeThrowForce;
    public GameObject bladePrefab;
    public GameObject bladePivotPrefab;
    public List<GameObject> activeBlades; //Do i need a list for this?
    public float bladesRotationSpeed;
    public static Transform instance; //Curent player physcial space/location for other parts of the program
    public bool gameOver = false;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 mouseScreenPosition; //Vector of where mouse is on screen in relation to camera
    private float xBounds = 25;
    private float zBounds = 25;
    private float currentFireRateTime;
    private GameObject bladePivotInstance;

    //NOTE: Currently in the prefab I have a canvas child obejct for health bar, IT SHOULD NOT BE THERE for editing purposes and future projects

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = transform;
        bladePivotInstance = Instantiate(bladePivotPrefab, bladePivotPrefab.transform.position, Quaternion.identity);
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
            if (currentFireRateTime >= fireRateInterval)
            {
                ProjectileSpawn();
                currentFireRateTime = 0;
            }
        }
    }

    void LateUpdate()
    {
        bladePivotInstance.transform.position = transform.position;
        bladePivotInstance.transform.Rotate(Vector3.up, bladesRotationSpeed * Time.deltaTime);
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

        if (transform.position.z > zBounds)
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
            //Note: it will start spewing bullets like a beam when you reach a certain point
            if (powType == PowerUpType.FireRateBoost)
            {
                fireRateInterval -= .1f;
            }
            else if (powType == PowerUpType.DamageBoost)
            {
                damage++;
            }
            else if (powType == PowerUpType.GrenadePowerup)
            {
                Vector3 spawnPos = transform.position + transform.forward * 1f;
                GameObject thrownGrenade = Instantiate(grenade, spawnPos, transform.rotation);
                Rigidbody grenadeRb = thrownGrenade.GetComponent<Rigidbody>();
                grenadeRb.AddForce(transform.forward * grenadeThrowForce);
            }
            else if (powType == PowerUpType.SpinningBlades)
            {
                //Distance it rotates from pivot
                float radius = 4f;
                //instantiate object as child of pivot
                GameObject tempBlade = Instantiate(bladePrefab, bladePivotInstance.transform);
                //add it to current blade instance
                activeBlades.Add(tempBlade);
                

                for(int i = 0; i < activeBlades.Count; i++)
                {
                    //Get angle, convert it to radians, and then multiply it by radius to get proper position
                    //cos for x position, sin for z position
                    float angle = i * (360 / activeBlades.Count);
                    float angleRad = angle * Mathf.Deg2Rad;
                    activeBlades[i].transform.localPosition = new Vector3(radius * Mathf.Cos(angleRad), 0, radius * Mathf.Sin(angleRad));
                    activeBlades[i].transform.rotation = Quaternion.identity;
                }

                if (activeBlades.Count > 1)
                {
                    bladesRotationSpeed += 50;
                }
            }
        }
    }
}
