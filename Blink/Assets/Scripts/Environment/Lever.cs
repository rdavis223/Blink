﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("Disable");
    }

    public void Enable()
    {
        anim.Play("Enable");
    }

    public void Disable()
    {
        anim.Play("Disable");
    }

}
