using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    private bool stopped = false;
    public float speed = 1.0f;
    private Vector3 currentForward;

    public int damage = 200;

    public AudioSource moveSound;
    private AudioSource hitSound;

    private bool pulling = false;

    private bool soundInProgress = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).transform.Rotate(0, 0, -45);
        AudioSource[] sources = GetComponents<AudioSource>();
        moveSound = sources[0];
        hitSound = sources[1];

        moveSound.Play();
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
        if (other.tag == "Enemy")
        {
            if (other.name.Contains("Sniper"))
            {
                other.GetComponent<EnemyHealthManager>().HurtEnemy(damage, "Death_from_front", 3.433f);
            }
            else
            {
                other.GetComponent<EnemyHealthManager>().HurtEnemy(damage, "death_from_front", 4.33f);
            }
            hitSound.Play();
            return;
        } else

        if (other.tag =="EnemyHead")
        {
            if (other.name.Contains("Sniper"))
            {
                other.transform.parent.parent.parent.GetComponent<EnemyHealthManager>().InstantDeath("death_from_front_headshot", 2.83f);

            }
            else
            {
                other.transform.parent.parent.GetComponent<EnemyHealthManager>().InstantDeath("head_shot", 2.83f);
            }
            hitSound.Play();
            return;
        } else if (other.tag == "ShieldBack")
        {
            other.transform.parent.parent.GetComponent<EnemyHealthManager>()?.HurtEnemy(3000, "death", 2.96f);
        }
        else if (other.tag == "EnemyBullet" || other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            return;
        }

        else if (other.GetComponent<Collider>().gameObject.name != "Player" && other.GetComponent<Collider>().gameObject.name != "Shuriken" && (other.GetComponent<Collider>().gameObject.transform.parent == null || (other.GetComponent<Collider>().gameObject.transform.parent != null && other.GetComponent<Collider>().gameObject.transform.parent.name != "Player")))
        {
            hitSound.Play();
            stopped = true;
        }
        
    }

}
