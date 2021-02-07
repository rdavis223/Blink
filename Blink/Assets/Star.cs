using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    private bool stopped = false;
    public float speed = 1.0f;
    private Vector3 currentForward;

    public int damage = 200;

    private bool pulling = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).transform.Rotate(0, 0, -45);

    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped){
            transform.position += transform.forward * speed * Time.deltaTime;
            transform.GetChild(0).transform.Rotate(0, 400* Time.deltaTime, 0);
        } else if (pulling){
            transform.GetChild(0).transform.Rotate(0, -400* Time.deltaTime, 0);
        }
        
    }

    public void SetStop(){
        stopped = true;
        pulling = true;
        transform.GetChild(0).transform.Rotate(0, 0, 90);
    }
    void OnTriggerEnter(Collider other){
        Damageable damageable = other.gameObject.GetComponent<Damageable>();

        if (damageable)
        {
            damageable.Damage(damage);
        }
        else if (other.GetComponent<Collider>().gameObject.name != "PlayerController" && other.GetComponent<Collider>().gameObject.name != "Shuriken"){
            stopped = true;
        }
        
    }
}
