using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1000f;

    public float lifetime = 3f;

    public int Damage = 25;

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
        print(CreatorMask == PlayerMask);
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
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable)
        {
            LayerMask OtherMask = 1 << other.gameObject.layer;
            if ((CreatorMask & OtherMask) == CreatorMask)
            {
                return;
            }
            damageable.Damage(Damage);
        }
    }
}
