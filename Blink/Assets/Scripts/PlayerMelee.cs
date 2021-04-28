using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)){
            Collider[] hitColliders = Physics.OverlapSphere((gameObject.transform.position + gameObject.transform.forward), 1f);
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.name.Contains("Enemy")){
                    hitCollider.gameObject.GetComponent<EnemyHealthManager>().HurtEnemy(125, "death_from_front", 4.33f);
                }
            }
        }
    }
}
