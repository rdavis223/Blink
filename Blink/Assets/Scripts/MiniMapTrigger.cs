using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapTrigger : MonoBehaviour
{
    public List<GameObject> floorImgs;
    public Image objective;
    private int objectiveFloor;
    public GameObject player;
    public GameObject lastHit;
    public Vector3 collision = Vector3.zero;

    void Start()
    {

    }

    void Update()
    {
        var ray = new Ray(player.transform.position, -player.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;
            objectiveFloor = PlayerPrefs.GetInt("objectiveFloor");

            if (lastHit.transform.root.name == "Floor0")
            {
                for (int i = 0; i < floorImgs.Count; i++)
                {
                    floorImgs[i].SetActive(false);
                }

                floorImgs[0].SetActive(true);
                if (objectiveFloor == 0)
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
                }

                else
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.5f);
                }
            }

            if (lastHit.transform.root.name == "Floor1")
            {
                for (int i = 0; i < floorImgs.Count; i++)
                {
                    floorImgs[i].SetActive(false);
                }
                
                floorImgs[1].SetActive(true);
                if (objectiveFloor == 1)
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
                }

                else
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.5f);
                }
            }

            if (lastHit.transform.root.name == "Floor2")
            {
                for (int i = 0; i < floorImgs.Count; i++)
                {
                    floorImgs[i].SetActive(false);
                }
                
                floorImgs[2].SetActive(true);
                if (objectiveFloor == 2)
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
                }

                else
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.5f);
                }
            }

            if (lastHit.transform.root.name == "Floor3")
            {
                for (int i = 0; i < floorImgs.Count; i++)
                {
                    floorImgs[i].SetActive(false);
                }
                
                floorImgs[3].SetActive(true);
                if (objectiveFloor == 3)
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
                }

                else
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.5f);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(collision, 0.2f);
    }
}
