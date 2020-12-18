using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damageToGive = 1;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") { // Check for player
            FindObjectOfType<HealthManager>().HurtPlayer(damageToGive); // Hurt player
        }
    }
}
