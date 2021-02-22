using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{

    [SerializeField] int MaxHealth = 100;
    
    private int CurrentHealth;

    public UnityEvent OnDeath;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void Damage(int amount)
    {
        //print("Health: " + CurrentHealth.ToString() + " damaged to be done: " + amount.ToString());
        CurrentHealth -= amount;
        if(CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }

    public void InstantDeath()
    {
        OnDeath.Invoke();
    }
}
