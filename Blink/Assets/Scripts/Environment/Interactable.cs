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
    private float hoverCooldown = 5f;
    public GameObject hoverEnable;

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
        if (hoverEnable != null)
        {
            hoverEnable.SetActive(false);
        }
    }

    public void Hover()
    {
        if (hoverEnable != null)
        {
            hoverEnable.SetActive(true);
            Invoke(nameof(ResetPopup), hoverCooldown);
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

    private void ResetPopup()
    {
        if (hoverEnable != null)
        {
            hoverEnable.SetActive(false);
        }
    }
}
