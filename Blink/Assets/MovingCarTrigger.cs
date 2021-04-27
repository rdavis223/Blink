using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCarTrigger : MonoBehaviour
{

    private MovingPlatform carParent;
    // Start is called before the first frame update
    void Start()
    {
        carParent = this.transform.parent.transform.parent.gameObject.GetComponent<MovingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            carParent.attach(other.gameObject); // Lock object onto platform
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            carParent.detach();
        }
    }
}

