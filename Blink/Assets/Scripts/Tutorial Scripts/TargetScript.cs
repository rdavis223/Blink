using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public GameObject destroyPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GameObject effect = Instantiate(destroyPrefab);
            effect.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 3f, gameObject.transform.position.z);
            Destroy(gameObject);
            Destroy(other.gameObject);
            Destroy(effect, 2);
        }
    }
}
