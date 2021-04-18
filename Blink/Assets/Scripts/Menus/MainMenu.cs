using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int level_index;
    private float x;
    // Play
    public void Play() {

        level_index = PlayerPrefs.GetInt("current_level");

        if (level_index == 0)
        {
            level_index = 1;
        }
        SceneManager.LoadScene(level_index);


        //PlayerPrefs.SetInt("current_level", nextLevelIndex); Put this at in objective script
    }

    // Quit
    public void Quit()
    {
        Application.Quit();
    }
}