using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePannel;

    public GameObject blinkOverlay;

    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            togglePaused();
        }
    }

    public void togglePaused(){
        isPaused = !isPaused;
        pausePannel.SetActive(isPaused);
        if (isPaused){
            BlinkMgr.Instance.pauseMenuActive = true;
            if (BlinkMgr.Instance.BlinkActive){
                blinkOverlay.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        } else {
            BlinkMgr.Instance.pauseMenuActive = false;
            if (BlinkMgr.Instance.BlinkActive){
                blinkOverlay.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void quitGame(){
        SceneManager.LoadScene(0);
    }
}
