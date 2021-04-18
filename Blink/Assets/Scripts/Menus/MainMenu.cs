﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private float x;
    // Play
    public void Play() {
        SceneManager.LoadScene(1);
    }

    // Quit
    public void Quit()
    {
        Application.Quit();
    }
}