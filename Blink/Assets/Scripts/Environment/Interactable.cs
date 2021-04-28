using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onEnable;
    public UnityEvent onDisable;

    private bool isEnabled = false;
    private bool canInteract = true;
    private float interactCooldown = 1.5f;

    public void Start()
    {
        if (onEnable == null)
        {
            onEnable = new UnityEvent();
        }
        
        if (onDisable == null)
        {
            onDisable = new UnityEvent();
        }
    }

    public void EnableObj()
    {
        if (canInteract)
        {
            canInteract = false;
            onEnable.Invoke();
            isEnabled = true;
            Invoke(nameof(ResetInteract), interactCooldown);
        }

    }

    public void DisableObj()
    {
        if (canInteract)
        {
            canInteract = false;
            onDisable.Invoke();
            isEnabled = false;
            Invoke(nameof(ResetInteract), interactCooldown);
        }
    }

    public void Interact()
    {
        if (isEnabled)
        {
            DisableObj();
        }
        else
        {
            EnableObj();
        }
    }

    private void ResetInteract()
    {
        canInteract = true;
    }
}
