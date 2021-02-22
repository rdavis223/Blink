using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public GameObject healthBarUI;
    public Slider healthBarSlider;

    public GameObject deathEffect;

    void Start()
    {
        currentHealth = maxHealth; // Set initial health
        // Initialize health bar
        healthBarSlider.value = currentHealth / maxHealth;
        healthBarUI.SetActive(false);
    }

    void Update()
    {
        healthBarSlider.value = currentHealth / maxHealth;

        if (currentHealth < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
    }
    public void HurtEnemy(int damage)
    {
        Debug.Log("hurt enemy");
        currentHealth -= damage; // Decrease health
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public void InstantDeath()
    {
        Debug.Log("instant death");
        Destroy();
    }

    public void Destroy()
    {
        Instantiate(deathEffect);
        deathEffect.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.3f, gameObject.transform.position.z);
        Destroy(gameObject);
        //Debug.Log("Death");
        Destroy(deathEffect, 2);
    }

}