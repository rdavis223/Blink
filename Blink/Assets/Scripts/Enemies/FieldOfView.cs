using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public Transform meleeTarget;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public GameObject player;

    [SerializeField] private EnemyAI enemy;
    [SerializeField] private EnemyHealthManager enemyHealth;

    private bool takingDamage;
    private bool targetCoverSet = false;
    private Vector3 targetCover;

    private float takingDamangeTimer = 0f;

    public bool playerSpotted = false;
    public bool forcePatrol = false;

    void Start()
    {
        player = GameObject.Find("WeaponHandler");
        Debug.Log(player.transform.position);
    }
    void Update()
    {
        if (!BlinkMgr.Instance.BlinkActive)
        {
            enemy.agent.enabled = true;
            enemy.animator.enabled = true;

            enemy.playerInSightRange = Physics.CheckSphere(transform.position, enemy.sightRange, enemy.whatIsPlayer);
            enemy.playerInAttackRange = Physics.CheckSphere(transform.position, enemy.attackRange, enemy.whatIsPlayer);
            enemy.playerInMeleeRange = Physics.CheckSphere(transform.position, enemy.meleeRange, enemy.whatIsPlayer);

            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            if (forcePatrol)
            {

                forcePatrol = false;
                enemy.Patrolling();
            }

            if (!enemy.playerInSightRange)
            {
                playerSpotted = false;
            }
            if (!(enemyHealth.currentHealth <= 0))
            {
                if (takingDamage)
                {
                    playerSpotted = true;
                    if (!targetCoverSet)
                    {
                        targetCover = enemy.findClosestCover();
                        if (targetCover != Vector3.zero)
                        {
                            targetCoverSet = true;

                        } else
                        {
                            takingDamage = false;
                        }
                    }
                    if (Mathf.Approximately(this.transform.position.x, targetCover.x) && Mathf.Approximately(this.transform.position.z, targetCover.z) && targetCoverSet)
                    {
                        enemy.animator.SetBool("Shooting", true);
                        takingDamangeTimer -= Time.deltaTime;
                        if (takingDamangeTimer <= 0f)
                        {
                            takingDamage = false;
                            targetCoverSet = false;
                            enemy.coverObj.GetComponent<CoverPoint>().setOccupied(false);
                            enemy.coverObj = null;
                            enemy.animator.SetBool("Shooting", false);

                        }
                        if (isPlayerInSight())
                        {
                            enemy.AttackPlayerMoving();
                        }
                    }
                    else if (targetCoverSet)
                    {
                        enemy.animator.SetBool("Chasing", false);
                        enemy.animator.SetBool("Shooting", false);
                        enemy.moveToCover(targetCover);
                        //maybe change this so only if in sight range
                        if (isPlayerInSight())
                        {
                            enemy.AttackPlayerMoving();
                        }
                    }
                    return;
                }

                if (!enemy.playerInSightRange && !enemy.playerInAttackRange)
                {
                    enemy.animator.SetBool("Chasing", false);

                    enemy.Patrolling();
                }

                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2 || playerSpotted)
                    {
                        float dToTarget = Vector3.Distance(transform.position, target.position);
                        
                        // Player spotted
                        if (!Physics.Raycast(transform.position, dirToTarget, dToTarget, obstacleMask) || playerSpotted)
                        {
                            playerSpotted = true;
                            viewRadius = 50;
                            viewAngle = 90;
                            
                            if (!(enemy.playerInAttackRange))
                            {
                                enemy.animator.SetBool("Chasing", true);
                                enemy.animator.SetBool("Shooting", false);
                                enemy.ChasePlayer();
                            }

                            if (enemy.playerInAttackRange && !(enemy.playerInMeleeRange))
                            {
                                if (playerSpotted)
                                {
                                    bool viewObstructed = !isPlayerInSight();
                                    if (viewObstructed || !enemy.playerInAttackRange)
                                    {
                                        enemy.animator.SetBool("Chasing", true);
                                        enemy.animator.SetBool("Shooting", false);
                                        enemy.ChasePlayer();
                                    }
                                    else
                                    {
                                        enemy.animator.SetBool("Shooting", true);
                                        enemy.AttackPlayer();
                                    }
                                }
                                else
                                {
                                    enemy.animator.SetBool("Shooting", true);
                                    enemy.AttackPlayer();
                                }
                            }

                            // Player in melee distance
                            if (enemy.playerInMeleeRange)
                            {
                                enemy.playerInAttackRange = false;
                                enemy.MeleeAttackPlayer();
                            }
                            else
                            {
                                enemy.animator.SetBool("Striking", false);
                            }
                        } else
                        {
                            enemy.animator.SetBool("Chasing", false);

                            enemy.Patrolling();
                        }


                    } else
                    {
                        enemy.animator.SetBool("Chasing", false);

                        enemy.Patrolling();
                    }
                }
            }
        } else
        {
            enemy.walkPointSet = false;
            playerSpotted = false;
            enemy.animator.SetBool("Chasing", false);
            enemy.animator.SetBool("Shooting", false);
            forcePatrol = true;

        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void SetTakingDamage()
    {
        takingDamage = true;
        takingDamangeTimer = 3f;
    }

    public bool isPlayerInSight()
    {
        float maxDist = Vector3.Distance(this.transform.position, player.transform.position);
        Vector3 dir = player.transform.position - this.transform.position;
        Ray r = new Ray(this.transform.position, dir);
        RaycastHit[] hits = Physics.RaycastAll(r, maxDist);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag == "Cover")
            {
                return false;

            }
        }
        return true;
    }

    public bool isLocationInSight(Vector3 location)
    {
        float maxDist = Vector3.Distance(this.transform.position, location);
        Vector3 dir = location - this.transform.position;
        Ray r = new Ray(this.transform.position, dir);
        RaycastHit[] hits = Physics.RaycastAll(r, maxDist);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag == "Cover")
            {
                return false;

            }
        }
        return true;
    }

    public void HearGunshots(Vector3 shotPos)
    {
        float distanceToShotPos = Vector3.Distance(shotPos, this.transform.position);
        if (distanceToShotPos < enemy.sightRange && isLocationInSight(shotPos) && !playerSpotted)
        {
            enemy.SetWalkPoint(shotPos);
            enemy.animator.SetBool("Chasing", false);

            enemy.Patrolling();
        }

    }

}
