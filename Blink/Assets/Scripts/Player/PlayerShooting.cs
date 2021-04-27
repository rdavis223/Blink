using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Camera playerCamera;
    
    public Transform ShootPosition;

    public Texture2D crosshairTexture;

    public float crosshairScale;

    public float fireRate = 0.2f;

    private float fireTimer = 0f;

    public int ammo;

    public int clipSize;

    public Transform ammoText = null;

    public int currentAmmo = 0;

    public int currentClip = 0; 

    private float reloadTimer;

    public float reloadSpeed;

    public bool reloading = false;

    public string fireType;

    public string fireStyle = "straight";

    public int numBullets = 1;

    public float accuracy = 1f;

    public int manuallySetAmmo = -1;

    public int manuallySetClip = -1;

    public GameObject dropObject;


    private AudioSource shootSound;
    private AudioSource reloadSound;


    void updateAmmoText(string textToUpdate){
        ammoText.GetComponent<TMP_Text>().text = textToUpdate;
    }

    void reload(){
        reloading = false;
        if (currentAmmo >= clipSize){
            int toRefill = clipSize - currentClip;
            currentAmmo -= toRefill;
            currentClip += toRefill;
        }
        else 
        {
            currentClip = currentAmmo;
            currentAmmo = 0;
        }
        fireTimer = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {   
        AudioSource[] sources = GetComponents<AudioSource>();
        shootSound = sources[0];
        reloadSound = sources[1];
        currentAmmo = ammo;
        reload();
    }

    void OnEnable()
    {
        if (ammoText != null){
            updateAmmoText(currentClip.ToString() + "/" + currentAmmo.ToString());
        }
    }
    
    void OnGUI(){
        if (Cursor.lockState == CursorLockMode.Locked){
            GUI.DrawTexture(new Rect((Screen.width-crosshairTexture.width*crosshairScale)/2 ,(Screen.height-crosshairTexture.height*crosshairScale)/2, crosshairTexture.width*crosshairScale, crosshairTexture.height*crosshairScale),crosshairTexture);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (manuallySetAmmo != -1){
            Debug.Log(manuallySetAmmo);
            currentAmmo = manuallySetAmmo; 
            manuallySetAmmo = -1;
        }
        if (manuallySetClip != -1){
            currentClip = manuallySetClip;
            manuallySetClip = -1;
        }
        if (!reloading){
            updateAmmoText(currentClip.ToString() + "/" + currentAmmo.ToString());
        }
        if (Cursor.lockState == CursorLockMode.Locked){
            if (fireTimer > 0f && fireType == "auto"){
                fireTimer -= Time.deltaTime;
            }

            if (reloadTimer > 0f){
                reloadTimer -= Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.R) && !(currentAmmo == 0 && currentClip == 0) && currentClip != clipSize){
                reloadTimer = reloadSpeed;
                reloading = true;
                reloadSound.Play();
                updateAmmoText("Reloading");
            }

            if (reloadTimer <= 0f && reloading){
                reload();
            }
            bool fireCondition = false;
            if (fireType == "auto"){
                fireCondition = Input.GetMouseButton(0);
            } else {
                fireCondition = Input.GetMouseButtonDown(0);
            }
            if (fireCondition && (fireTimer <= 0f || fireType != "auto") && !reloading && currentClip > 0){
                if (fireType == "auto"){
                    fireTimer = fireRate;
                }
                shootSound.Play();
                if (fireStyle == "spread"){
                    int i = 0;
                    while (i < numBullets){
                        GameObject bulletObject = Instantiate(bulletPrefab);
                        bulletObject.transform.position = ShootPosition.transform.position;
                        bulletObject.transform.forward = ShootPosition.transform.forward;
                        float end_x = playerCamera.transform.position.x + 100 * playerCamera.transform.forward.x;
                        float end_y = playerCamera.transform.position.y + 100 * playerCamera.transform.forward.y;
                        float end_z = playerCamera.transform.position.z + 100 * playerCamera.transform.forward.z;
                        Vector3 lookPosition = new Vector3(end_x, end_y, end_z);
                        bulletObject.transform.LookAt(lookPosition);
                        Vector3 offset = new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy));
                        Vector3 rotationVector = bulletObject.transform.eulerAngles + offset;
                        bulletObject.transform.rotation = Quaternion.Euler(rotationVector);
                        i++;
                    }
                } else {
                    GameObject bulletObject = Instantiate(bulletPrefab);
                    bulletObject.transform.position = ShootPosition.transform.position;
                    bulletObject.transform.forward = ShootPosition.transform.forward;
                    float end_x = playerCamera.transform.position.x + 100 * playerCamera.transform.forward.x;
                    float end_y = playerCamera.transform.position.y + 100 * playerCamera.transform.forward.y;
                    float end_z = playerCamera.transform.position.z + 100 * playerCamera.transform.forward.z;
                    Vector3 lookPosition = new Vector3(end_x, end_y, end_z);
                    bulletObject.transform.LookAt(lookPosition);
                }
                currentClip -= 1;

                                
            }
        }
    }
}
