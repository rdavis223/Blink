using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform playerRespawnPosition;
    public int checkpointObjectiveFloor;
    public int checkpointObjectiveNum;

    public void SetSpawnPosition()
    {
        PlayerPrefs.SetFloat("xpos", playerRespawnPosition.position.x);
        PlayerPrefs.SetFloat("ypos", playerRespawnPosition.position.y);
        PlayerPrefs.SetFloat("zpos", playerRespawnPosition.position.z);
        PlayerPrefs.SetInt("checkpointObjectiveFloor", checkpointObjectiveFloor);
        PlayerPrefs.SetInt("checkpointObjectiveNum", checkpointObjectiveNum);
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SetSpawnPosition();
        }
    }
}
