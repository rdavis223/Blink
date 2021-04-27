using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollowPlayer : MonoBehaviour
{
    public GameObject player;
    
    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
    }
 }
