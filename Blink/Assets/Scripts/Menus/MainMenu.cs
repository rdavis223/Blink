using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    
    private int level_index;
    private float x;


    private void Start()
    {
        if (PlayerPrefs.GetInt("current_level") == 0)
        {
            GameObject.Find("Continue").SetActive(false);
        }
    }
    // Play
    public void Play() {

        level_index = PlayerPrefs.GetInt("current_level");

        if (level_index == 0)
        {
            level_index = 1;
            PlayerPrefs.SetInt("current_level", 1);

        }
        StartCoroutine(LoadYourAsyncScene(level_index));
    }


    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        level_index = 1;
        PlayerPrefs.SetInt("current_level", 1);
        StartCoroutine(LoadYourAsyncScene(level_index));
    }

    public void Tut()
    {
        SceneManager.LoadScene(1);
    }   

    public void Lvl1()
    {
        SceneManager.LoadScene(2);
    }

    public void Lvl2()
    {
        SceneManager.LoadScene(3);
    }

    public void Lvl3()
    {
        SceneManager.LoadScene(4);
    }

    // Quit
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadYourAsyncScene(int scene)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}