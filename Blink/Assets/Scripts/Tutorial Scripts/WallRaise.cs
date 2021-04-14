using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRaise : MonoBehaviour
{
    private bool raised;
    public float speed;
    public Transform target;

    void Start()
    {
        raised = false;
    }

    void Update()
    {
        if (raised)
        {
            float step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !raised)
        {
            raised = true;
        }
    }
}
