using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.rotation = bulletPrefab.transform.rotation;
            Debug.Log(bulletObject.transform.rotation.eulerAngles);
            bulletObject.transform.localScale = new Vector3(50, 50, 50);
            bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            bulletObject.transform.forward = playerCamera.transform.forward;
            bulletObject.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);
            Debug.Log(bulletObject.transform.rotation.eulerAngles);


        }
    }
}
