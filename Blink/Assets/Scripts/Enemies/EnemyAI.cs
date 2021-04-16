using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    // Striking
    public int meleeDamage;
    private float lastAttackTime;
    public float attackDelay;

    public GameObject enemyLookPoint;

    // States
    public float sightRange, attackRange, meleeRange;
    public bool playerInSightRange, playerInAttackRange, playerInMeleeRange;

    public GameObject coverObj = null;

    private void Start()
    {
        
    }
    private void Awake()
    {
        enemyLookPoint = GameObject.Find("EnemyLookPoint");
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            findClosestCover();
        }
        // Check for sight and attack range
        if (!BlinkMgr.Instance.BlinkActive)
        {
            agent.enabled = true;
            animator.enabled = true;
        }
        else
        {
            agent.enabled = false;
            animator.enabled = false;
        }
    }
        

    public void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.x + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, 1 << 8))
            walkPointSet = true;
    }

    public void ChasePlayer()
    {
        agent.SetDestination(new Vector3(player.position.x, 0, player.position.z));
        transform.LookAt(new Vector3(player.position.x, 0, player.position.z));
        agent.transform.LookAt(new Vector3(player.position.x, 0, player.position.z));
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(enemyLookPoint.transform.position);
        agent.transform.LookAt(enemyLookPoint.transform.position);

        if (!alreadyAttacked)
        {
            // Attack code here
            Vector3 aim = (player.position - transform.position).normalized;
            GameObject bulletObject = Instantiate(projectile);
            bulletObject.transform.rotation = projectile.transform.rotation;
            bulletObject.transform.position = agent.transform.position + aim;
            bulletObject.transform.forward = aim;          
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void AttackPlayerMoving()
    {
        transform.LookAt(enemyLookPoint.transform.position);
        agent.transform.LookAt(enemyLookPoint.transform.position);

        if (!alreadyAttacked)
        {
            // Attack code here
            Vector3 aim = (player.position - transform.position).normalized;
            GameObject bulletObject = Instantiate(projectile);
            bulletObject.transform.rotation = projectile.transform.rotation;
            bulletObject.transform.position = agent.transform.position + aim;
            bulletObject.transform.forward = aim;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void MeleeAttackPlayer()
    {
        if (Time.time > lastAttackTime + attackDelay)
        {
            animator.SetBool("Striking", true);
            FindObjectOfType<HealthManager>().HurtPlayer(meleeDamage);
            lastAttackTime = Time.time;
            Debug.Log("hit!");
        }
    }

    public void moveToCover(Vector3 coverPos)
    {
        agent.SetDestination(coverPos);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }


    public Vector3 findClosestCover()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 40);
        IDictionary<float, GameObject> coverObjs = new Dictionary<float, GameObject>();
        List<float> coverDistances = new List<float>();
        foreach(Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "CoverPoint")
            {
                float dist = Vector3.Distance(hitCollider.gameObject.transform.position, this.transform.position);
                coverDistances.Add(dist);
                coverObjs.Add(dist, hitCollider.gameObject);
            }
        }
        coverDistances.Sort();
        foreach(float d in coverDistances)
        {
            float maxDist = Vector3.Distance(coverObjs[d].transform.position, player.position);
            Vector3 dir = coverObjs[d].transform.position - player.position;
            Ray r = new Ray(player.position, dir);
            RaycastHit[] hits = Physics.RaycastAll(r, maxDist);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.tag == "Cover")
                {
                    if (coverObjs[d].GetComponent<CoverPoint>().isFree())
                    {
                   
                        coverObj = coverObjs[d];
                        coverObj.GetComponent<CoverPoint>().setOccupied(true);
                        Debug.DrawLine(player.position, coverObj.transform.position, Color.red, 5f);
                        return coverObj.transform.position;
                    }
                    
                }
            }
        }
        return Vector3.zero;
    }

}


