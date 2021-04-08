using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    CapsuleCollider playerCol;
    float originalHeight;
    public float reducedHeight;

    void Start()
    {
        playerCol = GetComponent<CapsuleCollider>();
        originalHeight = playerCol.height;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Crouch();
        else if (Input.GetKeyUp(KeyCode.LeftControl))
            GoUp();
    }

    void Crouch()
    {
        playerCol.height = reducedHeight;

    }

    void GoUp()
    {
        playerCol.height = originalHeight;
    }

}
