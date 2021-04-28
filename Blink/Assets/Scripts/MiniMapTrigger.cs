using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapTrigger : MonoBehaviour
{
    public List<GameObject> floorImgs;
    public List<Image> objectives;
    private int objectiveFloor;
    public GameObject player;
    public GameObject lastHit;
    public Vector3 collision = Vector3.zero;
    private Image objective;
    public LayerMask layer;

    void Update()
    {
        var ray = new Ray(player.transform.position, -player.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 30, layer.value))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;
            objectiveFloor = PlayerPrefs.GetInt("objectiveFloor");
            objective = objectives[PlayerPrefs.GetInt("objectiveNum")];

            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].color = new Color(objective.color.r, objective.color.g, objective.color.b, 0f);
            }

            if (lastHit.transform.parent.name == "Floor0")
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
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.3f);
                }
            }

            if (lastHit.transform.parent.name == "Floor1")
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
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.3f);
                }
            }

            if (lastHit.transform.parent.name == "Floor2")
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
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.3f);
                }
            }

            if (lastHit.transform.parent.name == "Floor3")
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
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.3f);
                }
            }

            if (lastHit.transform.parent.name == "Floor4")
            {
                for (int i = 0; i < floorImgs.Count; i++)
                {
                    floorImgs[i].SetActive(false);
                }
                
                floorImgs[4].SetActive(true);
                if (objectiveFloor == 4)
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 1f);
                }

                else
                {
                    objective.color = new Color(objective.color.r, objective.color.g, objective.color.b, 0.3f);
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
