using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public GameObject floor1img;
    public GameObject floor0img;
    private bool floor1;
    private bool floor0;
    public Image objective;

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
                objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.5f);
            }

            else if (floor0)
            {
                floor1img.SetActive(true);
                floor0img.SetActive(false);
                floor1 = true;
                floor0 = false;
                objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
            }
        }
    }


}
