using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMiniMap : MonoBehaviour
{
    public GameObject floor1img;
    public GameObject floor0img;
    private bool floor1;
    private bool floor0;
    public Image objective;
    private int floor;
    private int currentFloor;

    void Start()
    {
        floor = PlayerPrefs.GetInt("floor");
        if (floor == 1)
        {
            Debug.Log("hi");
            floor1img.SetActive(false);
            floor0img.SetActive(true);
            objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
        }

        if (floor == 0)
        {
            Debug.Log("hii");
            floor1img.SetActive(true);
            floor0img.SetActive(false);
            objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print(floor);
        if (other.tag == "Player")
        {
            if (floor == 0)
            {
                Debug.Log("trigger 0");
                floor1img.SetActive(false);
                floor0img.SetActive(true);
                currentFloor = 1;
                objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
            }

            if (floor == 1)
            {
                Debug.Log("trigger 1");
                floor1img.SetActive(true);
                floor0img.SetActive(false);
                currentFloor = 0;
                objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.5f);
            }

            floor = currentFloor;
        }
    }


}
