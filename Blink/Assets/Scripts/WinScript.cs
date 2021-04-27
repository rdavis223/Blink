using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public int nextLevelIndex;
    public int nextLevelStartFloor;
    public int nextLevelObjectiveFloor;

    public void Win()
    {
        PlayerPrefs.SetFloat("xpos", 0f);
        PlayerPrefs.SetFloat("ypos", 0f);
        PlayerPrefs.SetFloat("zpos", 0f);
        PlayerPrefs.SetInt("current_level", nextLevelIndex);
        PlayerPrefs.SetInt("checkpointFloor", nextLevelStartFloor);
        PlayerPrefs.SetInt("objectiveFloor", nextLevelObjectiveFloor);
    }
}
