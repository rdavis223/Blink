using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar; 

    // Temporary invincibility after damaged
    public float invincibilityDuration;
    private float invincibilityTimer;
    private bool isInvincible;

    // Game Over Stuff
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    private float fadeTimer;
    public CanvasGroup gameOverScreen;
    public CanvasGroup gameOverOptions;
    private bool playerIsDead = false;
    public float deathDelay = 2f;

    // Damage Effect
    public float damageEffectFadeSpeed; 
    public float waitForDamageFade;
    public Image damageScreenEffect;
    private bool fadeIn;
    private bool fadeOut;
    public bool GodMode = false;

    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        gameOverScreen = GameObject.Find("Game Over Screen").GetComponent<CanvasGroup>();
        gameOverOptions = GameObject.Find("Game Over Menu").GetComponent<CanvasGroup>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth); // Set health bar
        gameOverOptions.blocksRaycasts = false; 
        gameOverScreen.alpha = 0;
    }

    void Update()
    {
        if (GodMode){
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
        if (playerIsDead) 
        {
            Die();
        }

        if (fadeIn) // Fade in the damaged screen effect
        {
            damageScreenEffect.color = new Color(damageScreenEffect.color.r, damageScreenEffect.color.g, damageScreenEffect.color.b,
                Mathf.MoveTowards(damageScreenEffect.color.a, 1f, damageEffectFadeSpeed * Time.deltaTime));

            if (damageScreenEffect.color.a == 1f)
            {
                fadeIn = false;
            }
        }

        if (fadeOut) // Fade out the damaged screen effect
        {
            damageScreenEffect.color = new Color(damageScreenEffect.color.r, damageScreenEffect.color.g, damageScreenEffect.color.b,
                Mathf.MoveTowards(damageScreenEffect.color.a, 0f, damageEffectFadeSpeed * Time.deltaTime));

            if (damageScreenEffect.color.a == 0f)
            {
                fadeOut = false;
            }
        }

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;

            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    public void HurtPlayer(int damage)
    {
        if (!isInvincible) // Only damage player if they're no longer invincible
        {
            currentHealth -= damage; // Decrease health
            healthBar.SetHealth(currentHealth); // Set healthbar UI
            invincibilityTimer = invincibilityDuration;
            isInvincible = true; 

            StartCoroutine(DamageEffectCoroutine());

            if (currentHealth <= 0) // Player dies
            {
                StartCoroutine(DeathDelayCoroutine()); 
            }
        }
    }

    public void MeleeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                StartCoroutine(DeathDelayCoroutine());
            }
        }
    }

    public void InstantDeath()
    {
        StartCoroutine(DeathDelayCoroutine()); 
    }

    private IEnumerator DamageEffectCoroutine() // Show the damage effect
    {
        fadeIn = true;
        yield return new WaitForSeconds(waitForDamageFade);
        fadeIn = false;
        fadeOut = true;
    }

    public void Die()
    {
        // Fade to game over screen
        Cursor.lockState = CursorLockMode.None;
        fadeTimer += Time.deltaTime;
        gameOverScreen.alpha = fadeTimer / fadeDuration;

        if (fadeTimer > fadeDuration + displayImageDuration)
        {
            gameOverOptions.blocksRaycasts = true;
            gameOverOptions.alpha = 1; // Show game over menu
        }
    }

    // Delay the fade
    private IEnumerator DeathDelayCoroutine()
    {
        yield return new WaitForSeconds(deathDelay);
        playerIsDead = true;
    }
}
