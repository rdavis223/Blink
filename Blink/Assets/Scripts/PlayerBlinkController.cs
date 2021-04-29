using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlinkController : MonoBehaviour
{

    public GameObject blinkOverlay;
    // Start is called before the first frame update
    void Start()
    {
        BlinkMgr.Instance.BlinkTimer = 3f;
        BlinkMgr.Instance.BlinkActive = false;
        BlinkMgr.Instance.pauseMenuActive = false;
        BlinkMgr.Instance.tutorialPromptActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (BlinkMgr.Instance.BlinkTimer <= 0){
            BlinkMgr.Instance.BlinkActive = false;
            blinkOverlay.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !BlinkMgr.Instance.pauseMenuActive && !BlinkMgr.Instance.tutorialPromptActive){
            blinkOverlay.SetActive(!BlinkMgr.Instance.BlinkActive);
            BlinkMgr.Instance.BlinkActive = !BlinkMgr.Instance.BlinkActive;
        }
        if (BlinkMgr.Instance.BlinkActive){
            BlinkMgr.Instance.BlinkTimer -= Time.deltaTime;
        } else if (BlinkMgr.Instance.BlinkTimer < 3f){
            BlinkMgr.Instance.BlinkTimer += Time.deltaTime*0.35f;
        }
    }
}
