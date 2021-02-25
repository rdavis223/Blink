using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;
    public float maxStamina;
    private float currentStamina;
    private bool useStamina;
    public static StaminaBar instance;

    [Header("Higher number = slower speed")]
    public float rechargeSpeed;

    [Header("Pause before recharging")]
    public float waitTime;

    private Coroutine regenerate;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void Update()
    {
        if (useStamina)
        {
            currentStamina -= Time.deltaTime;
            staminaBar.value = currentStamina;

            if (regenerate != null)
            {
                StopCoroutine(regenerate);
            }

            regenerate = StartCoroutine(RegenerateStamina());
        }
    }

    public void UseStamina(bool use)
    {
        useStamina = use;
    }

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    private IEnumerator RegenerateStamina()
    {
        yield return new WaitForSeconds(waitTime);

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / rechargeSpeed;
            staminaBar.value = currentStamina;
            yield return new WaitForSeconds(0.01f);
        }
        regenerate = null;
    }
}
