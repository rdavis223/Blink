using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponMgr : MonoBehaviour
{

    public GameObject primaryPrefab;
    public GameObject secondaryPrefab;

    public GameObject primary;
    public GameObject secondary;

    public Camera playerCamera;

    public GameObject currentSelectedWeapon;

    public Transform ammoText;

    void initalizeWeapon(ref GameObject weaponPrefab, ref GameObject weapon, bool active){
        weapon = Instantiate(weaponPrefab);
        weapon.GetComponent<PlayerShooting>().ammoText = ammoText;
        weapon.GetComponent<PlayerShooting>().playerCamera = playerCamera;
        weapon.transform.SetParent(this.transform);
        weapon.SetActive(active);
        weapon.transform.localPosition = weaponPrefab.transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        initalizeWeapon(ref primaryPrefab, ref primary, true); 
        currentSelectedWeapon = primary;     
        if (secondaryPrefab != null){
            initalizeWeapon(ref secondaryPrefab, ref secondary, false); 
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            Debug.Log("switch");
            //switch weapons
            if (currentSelectedWeapon.GetComponent<PlayerShooting>().reloading){
                currentSelectedWeapon.GetComponent<PlayerShooting>().reloading = false;
            }
            if (currentSelectedWeapon == primary){
                primary.SetActive(false);
                secondary.SetActive(true);
                currentSelectedWeapon = secondary;
            } else {
                secondary.SetActive(false);
                primary.SetActive(true);
                currentSelectedWeapon = primary;
            }

        }

    }
}
