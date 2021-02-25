using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private GameObject WeaponHandler;
    public int AmmoToAdd = 20;
    // Start is called before the first frame update
    void Start()
    {
        WeaponHandler = GameObject.Find("WeaponHandler");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if (other.gameObject.name == "PlayerController"){
            GameObject currentWeapon = WeaponHandler.GetComponent<PlayerWeaponMgr>().primary;
            currentWeapon.GetComponent<PlayerShooting>().currentAmmo += AmmoToAdd;
            if (currentWeapon.GetComponent<PlayerShooting>().currentAmmo > currentWeapon.GetComponent<PlayerShooting>().ammo) {
                currentWeapon.GetComponent<PlayerShooting>().currentAmmo = currentWeapon.GetComponent<PlayerShooting>().ammo;
            }

            Destroy(this.gameObject);
        }
    }
}
