using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Camera playerCamera;

    public Texture2D crosshairTexture;

    public float crosshairScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnGUI(){
        GUI.DrawTexture(new Rect((Screen.width-crosshairTexture.width*crosshairScale)/2 ,(Screen.height-crosshairTexture.height*crosshairScale)/2, crosshairTexture.width*crosshairScale, crosshairTexture.height*crosshairScale),crosshairTexture);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.rotation = bulletPrefab.transform.rotation;
            bulletObject.transform.localScale = new Vector3(1, 1, 1);
            bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            bulletObject.transform.forward = playerCamera.transform.forward;
            bulletObject.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);
        }
    }
}
