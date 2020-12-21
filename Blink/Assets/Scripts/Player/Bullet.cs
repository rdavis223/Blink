using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1000f;

    public float lifetime = 3f;

    private float lifetimer;
    // Start is called before the first frame update
    void Start()
    {
        lifetimer = lifetime;
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        //make the bullet move
        Vector3 newSpeed = transform.forward * speed * Time.deltaTime;
        transform.position += transform.forward * speed * Time.deltaTime;
        lifetimer -= Time.deltaTime;
        if (lifetimer <= 0f) {
            Destroy(this.gameObject);
        }
    }
}
