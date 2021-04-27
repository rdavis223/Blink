using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    public GameObject weaponPrefab;
    public GameObject pickupText;

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
            if (current.gameObject.name == "Player"){
                WeaponHandler.GetComponent<PlayerWeaponMgr>().swapWeapons(transform.position, weaponPrefab, currentAmmo, currentClip);
                Destroy(this.gameObject);
        }
        }
    }

    void OnTriggerEnter(Collider other){
        pickupText.SetActive(true);
        current = other;

    }

    void OnTriggerExit(Collider other){
        pickupText.SetActive(false);
        current = null;
    }
}
