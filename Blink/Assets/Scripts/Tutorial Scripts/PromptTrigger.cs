using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptTrigger : MonoBehaviour
{
    public GameObject prompt;
    public GameObject blurBG;
    private bool isOpened;

    public void openPrompt()
    {
        if (FindObjectOfType<PauseMenu>() != null)
        {
            FindObjectOfType<PauseMenu>().disablePauseMenu();
        }
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        prompt.SetActive(true);
        blurBG.SetActive(true);
    }

    public void closePrompt()
    {
        if (FindObjectOfType<PauseMenu>() != null)
        {
            FindObjectOfType<PauseMenu>().enablePauseMenu();
        }
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        prompt.SetActive(false);
        blurBG.SetActive(false);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            openPrompt();
        }
    }
}
