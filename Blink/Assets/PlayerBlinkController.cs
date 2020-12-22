﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlinkController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (BlinkMgr.Instance.BlinkTimer <= 0){
            BlinkMgr.Instance.BlinkActive = false;
        }
        if (Input.GetKeyDown(KeyCode.Q)){
            BlinkMgr.Instance.BlinkActive = !BlinkMgr.Instance.BlinkActive;
        }
        if (BlinkMgr.Instance.BlinkActive){
            BlinkMgr.Instance.BlinkTimer -= Time.deltaTime;
        } else {
            BlinkMgr.Instance.BlinkTimer += Time.deltaTime*0.2f;
        }
    }
}
