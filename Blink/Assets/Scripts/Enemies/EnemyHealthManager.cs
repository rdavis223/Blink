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
        if (this.gameObject.name.Contains("Sniper"))
        {
            anim = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        }
        else
        {
            anim = gameObject.GetComponent<Animator>();
        }
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
        if (!this.gameObject.name.Contains("Sniper"))
        {
            this.gameObject.GetComponent<FieldOfView>().SetTakingDamage();

        }
        if (currentHealth <= 0)
        {

            anim.Play(animationName);
            StartCoroutine(DeathAnimationCoroutine(deathAnimTime));
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
        if (chance <= 15f){
            GameObject box = Instantiate(healthbox);
            box.transform.position = this.transform.position;
        } else if (chance > 15f && chance <= 30f){
            GameObject box = Instantiate(ammobox);
            box.transform.position = this.transform.position;
        }
    }

    private void DisableMovement()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        if (!this.gameObject.name.Contains("Sniper") && GetComponent<EnemyAI>().coverObj != null)
        {
            GetComponent<EnemyAI>().coverObj.GetComponent<CoverPoint>().setOccupied(false);
        }
    }
}