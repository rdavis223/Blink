using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject floor1img;
    public GameObject floor0img;
    private bool floor1;
    private bool floor0;

    void Start()
    {
        floor1img.SetActive(true);
        floor1 = true;
        floor0img.SetActive(false);
        floor0 = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (floor1)
            {
                floor1img.SetActive(false);
                floor0img.SetActive(true);
                floor1 = false;
                floor0 = true;
            }

            else if (floor0)
            {
                floor1img.SetActive(true);
                floor0img.SetActive(false);
                floor1 = true;
                floor0 = false;
            }
        }
    }


}
