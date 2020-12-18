using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    
    public void SetHealth(int health)
    {
        slider.value = health; // Set health on healthbar
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health; // Set max health on healthbar to be equal to player max health
        slider.value = health;
    }
}
