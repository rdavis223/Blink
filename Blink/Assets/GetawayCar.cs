using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetawayCar : MonoBehaviour
{
    private MovingPlatform mv;
    // Start is called before the first frame update
    void Start()
    {
        mv = this.GetComponent<MovingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.z <= 10 && mv.attachedObject.name == "Player")
        {
            Debug.Log("Win");
        }
    }
}
