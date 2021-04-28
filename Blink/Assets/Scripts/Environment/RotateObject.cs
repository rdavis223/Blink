using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        if (!BlinkMgr.Instance.BlinkActive)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
        }
    }
}
