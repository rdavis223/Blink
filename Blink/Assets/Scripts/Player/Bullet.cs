using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1000f;

    public float lifetime = 3f;

    const int DAMAGE = 25;

    public LayerMask CreatorMask; // Disallow self-collisions


    private float lifetimer;
    private bool HasTrailBeenFaked = false;
    private TrailRenderer BulletTrail;
    private float InitialBulletTrailTime;


    void Start()
    {
        BulletTrail = GetComponent<TrailRenderer>();
        InitialBulletTrailTime = BulletTrail.time;
        lifetimer = lifetime;
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (BlinkMgr.Instance.BlinkActive)
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
            damageable.Damage(DAMAGE);
        }
    }
}
