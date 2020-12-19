using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Camera playerCamera;

    public Texture2D crosshairTexture;

    public float crosshairScale;

    public float fireRate = 0.2f;

    private float fireTimer = 0f;

    public int ammo;

    public int clipSize;

    public Transform ammoText;

    private int currentAmmo = 0;

    private int currentClip = 0; 

    private float reloadTimer;

    public float reloadSpeed;

    private bool reloading;

    void updateAmmoText(string textToUpdate){
        ammoText.GetComponent<Text>().text = textToUpdate;
    }

    void reload(){
        reloading = false;
        if (currentAmmo >= clipSize){
            currentAmmo -= clipSize;
            currentClip += clipSize;
        }
        else 
        {
            currentClip = currentAmmo;
            currentAmmo = 0;
        }
        fireTimer = 0f;
        updateAmmoText(currentClip.ToString() + "/" + currentAmmo.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {   
        currentAmmo = ammo;
        reload();
    }
    
    void OnGUI(){
        GUI.DrawTexture(new Rect((Screen.width-crosshairTexture.width*crosshairScale)/2 ,(Screen.height-crosshairTexture.height*crosshairScale)/2, crosshairTexture.width*crosshairScale, crosshairTexture.height*crosshairScale),crosshairTexture);
    }
    // Update is called once per frame
    void Update()
    {
        if (fireTimer > 0f){
            fireTimer -= Time.deltaTime;
        }

        if (reloadTimer > 0f){
            reloadTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.R) && !(currentAmmo == 0 && currentClip == 0)){
            reloadTimer = reloadSpeed;
            reloading = true;
            updateAmmoText("Reloading");
        }

        if (reloadTimer <= 0f && reloading){
            reload();
        }

        if (Input.GetMouseButton(0) && fireTimer <= 0f && reloadTimer <= 0 && currentClip > 0){
            fireTimer = fireRate;
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.rotation = bulletPrefab.transform.rotation;
            bulletObject.transform.localScale = new Vector3(1, 1, 1);
            bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            bulletObject.transform.forward = playerCamera.transform.forward;
            bulletObject.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);
            currentClip -= 1;
            updateAmmoText(currentClip.ToString() + "/" + currentAmmo.ToString());
        }
    }
}
