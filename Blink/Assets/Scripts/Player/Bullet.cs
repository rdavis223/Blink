using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1000f;

    public float lifetime = 3f;

    // Damage (add more variables accordingly for weakspots)
    public int bodyDamage;
    public int backDamage;

    public LayerMask CreatorMask; // Disallow self-collisions
    public LayerMask PlayerMask;

    private float lifetimer;
    private bool HasTrailBeenFaked = false;
    private TrailRenderer BulletTrail;
    private float InitialBulletTrailTime;
    private bool isBlinkable;


    void Start()
    {
        BulletTrail = GetComponent<TrailRenderer>();
        InitialBulletTrailTime = BulletTrail.time;
        lifetimer = lifetime;
        transform.position += transform.forward * speed * Time.deltaTime;
        isBlinkable = CreatorMask != PlayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlinkable && BlinkMgr.Instance.BlinkActive)
        {
            if (!HasTrailBeenFaked)
            {
                transform.position += transform.forward * speed * Time.deltaTime * 4;
                HasTrailBeenFaked = true;
            }
            if (BulletTrail)
            {
                BulletTrail.time += Time.deltaTime;
            }
            return;
        }
        if (BulletTrail)
        {
            BulletTrail.time = InitialBulletTrailTime;
        }
        HasTrailBeenFaked = false;
        //make the bullet move
        Vector3 newSpeed = transform.forward * speed * Time.deltaTime;
        transform.position += transform.forward * speed * Time.deltaTime;
        lifetimer -= Time.deltaTime;
        if (lifetimer <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Enemy general body damage
        if (other.tag == "EnemyBody")
        {
            LayerMask OtherMask = 1 << other.gameObject.layer;
            if ((CreatorMask & OtherMask) == CreatorMask)
            {
                return;
            }
            other.GetComponent<EnemyHealthManager>().HurtEnemy(bodyDamage);
        }

        // Enemy HeadShot
        if (other.tag == "EnemyHead")
        {
            LayerMask OtherMask = 1 << other.gameObject.layer;
            if ((CreatorMask & OtherMask) == CreatorMask)
            {
                return;
            }
            other.transform.root.GetComponent<EnemyHealthManager>().InstantDeath();
        }

        // Enemy Back damage
        if (other.tag == "enemyBack")
        {
            LayerMask OtherMask = 1 << other.gameObject.layer;
            if ((CreatorMask & OtherMask) == CreatorMask)
            {
                return;
            }
            other.transform.root.GetComponent<EnemyHealthManager>().HurtEnemy(backDamage);
        }

        // ADD MORE IF STATEMENTS ACCORDINGLY FOR EACH DIFFERENT DAMAGE, TAG COLLIDER WITH A DESCRIPTIVE NAME
        // other.transform.root gets root parent in order to find the enemy health manager script
    }
}
