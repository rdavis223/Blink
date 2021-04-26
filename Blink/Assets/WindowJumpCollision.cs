using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowJumpCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (this.transform.parent.gameObject.GetComponent<EscapeController>().objectiveRetrieved)
            {
                this.transform.parent.gameObject.GetComponent<EscapeController>().StartCar();
                Destroy(this.gameObject);
            }
        }
    }
}
