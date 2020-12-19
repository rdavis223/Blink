using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Restart the game if the player presses restart
    public void Continue() {
        SceneManager.LoadScene(1);
    }

    // Go to main menu
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
