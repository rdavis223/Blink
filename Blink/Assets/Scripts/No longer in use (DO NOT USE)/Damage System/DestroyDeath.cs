using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDeath : MonoBehaviour
{
    public GameObject effect;

    public void Destroy()
    {
        GameObject deathEffect = Instantiate(effect);
        deathEffect.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.3f, gameObject.transform.position.z);
        Destroy(gameObject);
        //Debug.Log("Death");
        Destroy(deathEffect, 2);
    }
}
