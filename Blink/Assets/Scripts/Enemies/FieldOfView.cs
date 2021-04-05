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

    [SerializeField] private EnemyAI enemy;
    [SerializeField] private EnemyHealthManager enemyHealth;

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

            if (!(enemyHealth.currentHealth <= 0))
            {

                if (!enemy.playerInSightRange && !enemy.playerInAttackRange)
                {
                    enemy.animator.SetBool("Patrolling", true);
                    enemy.animator.SetBool("Chasing", false);

                    enemy.Patrolling();
                }

                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        float dToTarget = Vector3.Distance(transform.position, target.position);
                        
                        // Player spotted
                        if (!Physics.Raycast(transform.position, dirToTarget, dToTarget, obstacleMask))
                        {
                            viewRadius = 50;
                            viewAngle = 90;
                            
                            if (!(enemy.playerInAttackRange))
                            {
                                enemy.animator.SetBool("Chasing", true);
                                enemy.animator.SetBool("Shooting", false);
                                enemy.ChasePlayer();
                                Debug.Log("Shouldn't shoot!");
                            }

                            if (enemy.playerInAttackRange && !(enemy.playerInMeleeRange))
                            {
                                enemy.animator.SetBool("Shooting", true);
                                enemy.AttackPlayer();
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
                        }


                    }
                }
            }
            else 
            {
                enemy.agent.SetDestination(transform.position);
            }
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

}
