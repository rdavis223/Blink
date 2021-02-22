using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public GameObject healthBarUI;
    public Slider healthBarSlider;

    public GameObject effect;

    void Start()
    {
        currentHealth = maxHealth; // Set initial health
        // Initialize health bar
        healthBarSlider.value = currentHealth / maxHealth;
        healthBarUI.SetActive(false);
    }

    void Update()
    {
        // Update enemy health bar
        healthBarSlider.value = currentHealth / maxHealth;

        if (currentHealth < maxHealth) // Set healthbar active when you damage enemy
        {
            healthBarUI.SetActive(true);
        }
    }
    public void HurtEnemy(int damage)
    {
        // Hurt the enemy, decrease health
        currentHealth -= damage; // Decrease health
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public void InstantDeath()
    {
        // Instant death
        Destroy();
    }

    public void Destroy()
    {
        GameObject deathEffect = Instantiate(effect);
        deathEffect.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.3f, gameObject.transform.position.z);
        Destroy(gameObject);
        //Debug.Log("Death");
        Destroy(deathEffect, 2);
    }
}