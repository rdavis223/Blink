using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onEnable;
    public UnityEvent onDisable;

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
        onEnable.Invoke();
    }

    public void DisableObj()
    {
        onDisable.Invoke();
    }

}
