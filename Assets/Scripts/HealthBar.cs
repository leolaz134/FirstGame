using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public int maxHealth = 2;
    public float currentHealth;
    private GameObject EventSystem; //Im putting this just in case I want to do gamemanager/score stuff later
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        healthSlider.fillRect.gameObject.SetActive(true);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //decrements health and destroys object only if below 0 and its an enemy
    public void loseHealth(float amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        
        if (currentHealth <= 0 && transform.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
