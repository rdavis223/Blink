using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public int nextLevelIndex;
    public int nextLevelObjectiveFloor;
    public GameObject loadScreen;

    public bool lastLevel;

    public void Win()
    {
        PlayerPrefs.SetFloat("xpos", 0f);
        PlayerPrefs.SetFloat("ypos", 0f);
        PlayerPrefs.SetFloat("zpos", 0f);
        PlayerPrefs.SetString("PrimaryWeapon", "");
        PlayerPrefs.SetString("SecondaryWeapon", "");

        if (lastLevel)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);
        }
        else
        {
            PlayerPrefs.SetInt("current_level", nextLevelIndex);
            PlayerPrefs.SetInt("objectiveFloor", nextLevelObjectiveFloor);
            PlayerPrefs.SetInt("objectiveNum", 0);
        }
        loadScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(LoadYourAsyncScene(nextLevelIndex));
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
