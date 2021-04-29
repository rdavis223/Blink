using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform playerRespawnPosition;

    public GameObject WeaponHandler;
    private void Start()
    {
        WeaponHandler = GameObject.Find("WeaponHandler");
    }

    public void SetSpawnPosition(Vector3 playerPos)
    {
        //PlayerPrefs.SetFloat("xpos", playerRespawnPosition.position.x);
        //PlayerPrefs.SetFloat("ypos", playerRespawnPosition.position.y);
        //PlayerPrefs.SetFloat("zpos", playerRespawnPosition.position.z);

        PlayerPrefs.SetFloat("xpos", playerPos.x);
        PlayerPrefs.SetFloat("ypos", playerPos.y);
        PlayerPrefs.SetFloat("zpos", playerPos.z);
        PlayerPrefs.SetString("PrimaryWeapon", WeaponHandler.transform.GetChild(0).gameObject.name.Replace("(Clone)", ""));
        PlayerPrefs.SetString("SecondaryWeapon", WeaponHandler.transform.GetChild(1).gameObject.name.Replace("(Clone)", ""));
        PlayerPrefs.Save();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other.tag == "Player")
        {
            SetSpawnPosition(other.gameObject.transform.position);
            Debug.Log("checkpoint set");
        }
    }
}
