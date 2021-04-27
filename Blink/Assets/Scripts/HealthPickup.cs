using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private GameObject HealthManager;
    public int healthToAdd = 20;
    // Start is called before the first frame update
    void Start()
    {
        HealthManager = GameObject.Find("HealthManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if (other.tag == "Player"){
            HealthManager.GetComponent<HealthManager>().currentHealth += healthToAdd;
            if (HealthManager.GetComponent<HealthManager>().currentHealth > HealthManager.GetComponent<HealthManager>().maxHealth) {
                HealthManager.GetComponent<HealthManager>().currentHealth = HealthManager.GetComponent<HealthManager>().maxHealth;
            }
            Destroy(this.gameObject);
        }
    }
}
