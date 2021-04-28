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
    public bool walkPointSet;
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

    public GameObject Level;

    // States
    public float sightRange, attackRange, meleeRange;
    public bool playerInSightRange, playerInAttackRange, playerInMeleeRange;

    public GameObject coverObj = null;

    public GameObject shootPoint;

    private void Start()
    {
        
    }
    private void Awake()
    {
        enemyLookPoint = GameObject.Find("EnemyLookPoint");
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Level = GameObject.Find("Level");
    }

    private void Update()
    {
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
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
    }

    public void SearchWalkPoint()
    {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        NavMeshPath path = new NavMeshPath();
        if (Physics.Raycast(walkPoint, -transform.up, 2f, 1 << 8) && NavMesh.CalculatePath(this.transform.position, walkPoint, NavMesh.AllAreas, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            walkPointSet = true;
        }
    }

    public void SetWalkPoint(Vector3 newPos)
    {
        walkPoint = newPos;
        NavMeshPath path = new NavMeshPath();
        if (Physics.Raycast(walkPoint, -transform.up, 2f, 1 << 8) && NavMesh.CalculatePath(this.transform.position, walkPoint, NavMesh.AllAreas, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            walkPointSet = true;
        }
    }

    public void ChasePlayer()
    {
        agent.SetDestination(enemyLookPoint.transform.position);
        transform.LookAt(enemyLookPoint.transform.position);
        agent.transform.LookAt(enemyLookPoint.transform.position);
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(enemyLookPoint.transform.position);
        agent.transform.LookAt(enemyLookPoint.transform.position);

        if (!alreadyAttacked)
        {
            // Attack code here
            GetComponent<AudioSource>().Play();
            animator.Play("Shooting");
            Vector3 aim = (player.position - shootPoint.transform.position).normalized;
            GameObject bulletObject = Instantiate(projectile);
            bulletObject.transform.rotation = projectile.transform.rotation;
            bulletObject.transform.position = shootPoint.transform.position; 
            bulletObject.transform.forward = aim;          
            alreadyAttacked = true;
            //Level.BroadcastMessage("HearGunshots", this.transform.position);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        else
        {
            animator.SetBool("Shooting", false);
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
            bulletObject.transform.position = shootPoint.transform.position;
            bulletObject.transform.forward = aim;
            alreadyAttacked = true;
            //Level.BroadcastMessage("HearGunshots", this.transform.position);
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
                        NavMeshPath path = new NavMeshPath();
                        if (NavMesh.CalculatePath(this.transform.position, coverObjs[d].transform.position, NavMesh.AllAreas, path) && path.status == NavMeshPathStatus.PathComplete)
                        {
                            coverObj = coverObjs[d];
                            coverObj.GetComponent<CoverPoint>().setOccupied(true);
                            Debug.DrawLine(player.position, coverObj.transform.position, Color.green, 5f);
                            return coverObj.transform.position;
                        } 
                    }
                    
                }
            }
        }
        return Vector3.zero;
    }

}


