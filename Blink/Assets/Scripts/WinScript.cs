using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public void Win(int nextLevelIndex)
    {
        PlayerPrefs.SetFloat("xpos", 0f);
        PlayerPrefs.SetFloat("ypos", 0f);
        PlayerPrefs.SetFloat("zpos", 0f);
        PlayerPrefs.SetInt("current_level", nextLevelIndex);
    }
}
