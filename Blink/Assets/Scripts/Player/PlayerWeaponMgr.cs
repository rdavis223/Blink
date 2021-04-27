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
        weapon.transform.localRotation = weaponPrefab.transform.rotation;
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

    public void swapWeapons(Vector3 dropPos, GameObject newWeaponPrefab, int ammo, int clip){
        GameObject newDropWeapon = currentSelectedWeapon.GetComponent<PlayerShooting>().dropObject;
        int oldAmmo = currentSelectedWeapon.GetComponent<PlayerShooting>().currentAmmo;
        int oldClip = currentSelectedWeapon.GetComponent<PlayerShooting>().currentClip;
        if (currentSelectedWeapon == primary){
            Destroy(currentSelectedWeapon);
            primaryPrefab = newWeaponPrefab;
            initalizeWeapon(ref primaryPrefab, ref primary, true);
            if (ammo != -1){
                primary.GetComponent<PlayerShooting>().manuallySetAmmo = ammo;
                primary.GetComponent<PlayerShooting>().manuallySetClip = clip;
            }
            currentSelectedWeapon = primary;
        } else {
            Destroy(currentSelectedWeapon);
            secondaryPrefab = newWeaponPrefab;
            initalizeWeapon(ref secondaryPrefab, ref secondary, true);
            if (ammo != -1){
                secondary.GetComponent<PlayerShooting>().manuallySetAmmo = ammo;
                secondary.GetComponent<PlayerShooting>().manuallySetClip = clip;
            }
            currentSelectedWeapon = secondary;
        }

        GameObject drop = Instantiate(newDropWeapon);
        drop.GetComponent<WeaponPickup>().currentAmmo = oldAmmo;
        drop.GetComponent<WeaponPickup>().currentClip = oldClip;
        drop.transform.position = new Vector3(dropPos.x, drop.transform.position.y, dropPos.z);

    }
}
