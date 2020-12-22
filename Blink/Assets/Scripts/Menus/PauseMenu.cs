using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePannel;

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
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void quitGame(){
        SceneManager.LoadScene(0);
    }
}
