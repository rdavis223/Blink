using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    public GameObject weaponPrefab;

    private GameObject WeaponHandler;

    private Collider current = null;

    public int currentAmmo = -1;

    public int currentClip = -1;
    // Start is called before the first frame update
    void Start()
    {
        WeaponHandler = GameObject.Find("WeaponHandler");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && current != null){
            if (current.gameObject.name == "PlayerController"){
                WeaponHandler.GetComponent<PlayerWeaponMgr>().swapWeapons(transform.position, weaponPrefab, currentAmmo, currentClip);
                Destroy(this.gameObject);
        }
        }
    }

    void OnTriggerEnter(Collider other){
        current = other;

    }

    void OnTriggerExit(Collider other){
        current = null;
    }
}
