using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptTriggerNoTrigger : MonoBehaviour
{
    public GameObject prompt;
    public GameObject blurBG;
    private bool isOpened;
    public GameObject blinkOverlay;

    public void openPrompt()
    {
        BlinkMgr.Instance.tutorialPromptActive = true;
        if (BlinkMgr.Instance.BlinkActive)
        {
            blinkOverlay.SetActive(false);
        }
        FindObjectOfType<PauseMenu>().disablePauseMenu();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        prompt.SetActive(true);
        blurBG.SetActive(true);
    }

    public void closePrompt()
    {
        BlinkMgr.Instance.tutorialPromptActive = false;
        if (BlinkMgr.Instance.BlinkActive)
        {
            blinkOverlay.SetActive(true);
        }
        FindObjectOfType<PauseMenu>().enablePauseMenu();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        prompt.SetActive(false);
        blurBG.SetActive(false);
        Destroy(gameObject);
    }
}
