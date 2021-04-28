using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private bool isOpening = false;
    private bool isClosing = false;
    private float openSpeed = 10;
    private float timeToDestroy = 5;
    private Vector3 upperBounds;
    private Vector3 originalPos;
    public int numLocks;

    private void Start()
    {
        float height = this.GetComponent<MeshRenderer>().bounds.size.y;
        Vector3 pos = this.transform.position;
        originalPos = pos;
        upperBounds = new Vector3(pos.x, pos.y + height, pos.z);
    }

    public void Unlock()
    {
        numLocks -= 1;
        if (numLocks <= 0)
        {
            Open();
        }
    }

    public void Lock()
    {
        numLocks += 1;
        if (numLocks > 0)
        {
            Close();
        }
    }

    public void Open()
    {
        isOpening = true;
    }

    public void Close()
    {
        isClosing = true;
    }

    private void Update()
    {
        if (isOpening)
        {
            isClosing = false; // Cannot be opening and closing at the same time
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + openSpeed * Time.deltaTime, transform.position.z);
            if(newPos.y >= upperBounds.y)
            {
                isOpening = false;
                return;
            } 
            transform.position = newPos;
        }

        if (isClosing)
        {
            isOpening = false; // Cannot be opening and closing at the same time
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y - openSpeed * Time.deltaTime, transform.position.z);
            if (newPos.y <= originalPos.y)
            {
                isClosing = false;
                return;
            }
            transform.position = newPos;
        }
    }



}
