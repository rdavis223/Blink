using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    List<GameObject> listOfEnemies = new List<GameObject>();

    void Update()
    {
        if (EnemyHealthManager.deathCounter == 57)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
