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
        CurrentHealth -= amount;
        if(CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
