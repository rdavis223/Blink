using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyHealthManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public GameObject healthBarUI;
    public Slider healthBarSlider;

    public GameObject effect;

    private Animator anim;

    public GameObject healthbox;

    public GameObject ammobox; 
  
    void Start()
    {
        currentHealth = maxHealth; // Set initial health
        // Initialize health bar
        healthBarSlider.value = currentHealth / maxHealth;
        healthBarUI.SetActive(false);
        anim = gameObject.GetComponent<Animator>();
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

    public void HurtEnemy(int damage, string animationName, float deathAnimTime)
    {
        // Hurt the enemy, decrease health
        currentHealth -= damage; // Decrease health
        if (this.gameObject.name != "Sniper")
        {
            this.gameObject.GetComponent<FieldOfView>().SetTakingDamage();

        }
        if (currentHealth <= 0)
        {
            if (this.gameObject.name != "Sniper")
            {
                anim.Play(animationName);
                StartCoroutine(DeathAnimationCoroutine(deathAnimTime));

            } else
            {
                StartCoroutine(DeathAnimationCoroutine(1f));
            }
            DisableMovement();
        }
    }

    public void InstantDeath(string animationName, float deathAnimTime)
    {
        currentHealth = 0;
        anim.Play(animationName);
        // Instant death
        StartCoroutine(DeathAnimationCoroutine(deathAnimTime));
        DisableMovement();
    }

    public void Destroy()
    {
        GameObject deathEffect = Instantiate(effect);
        deathEffect.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
        DropItems(gameObject.transform.position);
        Destroy(gameObject);
        //Debug.Log("Death");
        Destroy(deathEffect, 2);
    }

    private IEnumerator DeathAnimationCoroutine(float deathAnimTime)
    {
        yield return new WaitForSeconds(deathAnimTime);
        Destroy();
    }
    private void DropItems(Vector3 position){
        float chance = Random.Range(1f,100f);
        Debug.Log(chance);
        if (chance <= 25f){
            GameObject box = Instantiate(healthbox);
            box.transform.position = new Vector3(position.x, healthbox.transform.position.y, position.z);
        } else if (chance > 25f && chance <= 50f){
            GameObject box = Instantiate(ammobox);
            box.transform.position = new Vector3(position.x, ammobox.transform.position.y, position.z);
        }
    }

    private void DisableMovement()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        if (this.gameObject.name != "Sniper" && GetComponent<EnemyAI>().coverObj != null)
        {
            GetComponent<EnemyAI>().coverObj.GetComponent<CoverPoint>().setOccupied(false);
        }
    }
}