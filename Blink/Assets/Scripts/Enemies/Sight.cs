using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sight : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    //// AI sighting behaviours
    //public float fieldOfViewAngle = 110f;
    //public bool playerInSight;
    //public Vector3 personalLastSighting;
    //private SphereCollider col;
    ////private LastPlayerSighting lastPlayerSighting;
    ////private PlayerHealth playerHealth;
    ////private HashIDs hash;
    ////private Vector3 previousSighting;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerController").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        

        //col = GetComponent<SphereCollider>();
        //lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
        //playerHealth = player.GetComponent<PlayerHealth>();
        //hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        //personalLastSighting = lastPlayerSighting.resetPosition;
        //previousSighting = lastPlayerSighting.resetPosition;
    }

    private void Update()
    {
        //if (lastPlayerSighting.position != previousSighting)
        //{
        //    personalLastSighting = lastPlayerSighting.position;
        //}

        //previousSighting = lastPlayerSighting.position;

        //if (playerHealth.health > 0f)
        //{
        //    animator.SetBool(hash.playerInSightBool, playerInSight);
        //}
        //else
        //{
        //    animator.SetBool(hash.playerInSightBool, false);
        //}
        // Check for sight and attack range
        if (!BlinkMgr.Instance.BlinkActive){
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
            {
                animator.SetBool("Patrolling", true);
                animator.SetBool("Spotted", false);
                animator.SetBool("Shooting", false);
                Patrolling();
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                animator.SetBool("Patrolling", false);
                animator.SetBool("Spotted", true);
                animator.SetBool("Shooting", false);
                ChasePlayer();
            }
            if (playerInSightRange && playerInAttackRange)
            {
                animator.SetBool("Patrolling", false);
                animator.SetBool("Spotted", false);
                animator.SetBool("Shooting", true);
                AttackPlayer();
            }
        }

        //if (!playerInSight && !playerInAttackRange)
        //{
        //    animator.SetBool("Patrolling", true);
        //    animator.SetBool("Spotted", false);
        //    animator.SetBool("Shooting", false);
        //}
        //if (playerInSight && !playerInAttackRange)
        //{
        //    animator.SetBool("Patrolling", false);
        //    animator.SetBool("Spotted", true);
        //    animator.SetBool("Shooting", false);
        //}
        //if (playerInSight && playerInAttackRange)
        //{
        //    animator.SetBool("Patrolling", false);
        //    animator.SetBool("Spotted", false);
        //    animator.SetBool("Shooting", true);
        //}

    }
    /**
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        //lastPlayerSighting.position = player.transform.position;
                    }
                }
            }

            //int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
            //int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).nameHash;
                
            //if (playerLayerZeroStateHash == hash.locomotionState || playerLayerOneStateHash == hash.shoutState)
            //{
            //    if (CalculatePathLength(player.transform.position) <= col.radius)
            //    {
            //        personalLastSighting = player.transform.position;
            //    }
            //}
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
        }
    }

    //float CalculatePathLength(Vector3 targetPosition)
    //{
    //    NavMeshPath path = new NavMeshPath();

    //    if (nav.enabled)
    //    {
    //        nav.CalculatePath(targetPosition, path);
    //    }

    //    Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
    //    allWayPoints[0] = transform.position;
    //    allWayPoints[allWayPoints.Length - 1] = targetPosition;

    //    for (int i = 0; i<path.corners.Length; i++)
    //    {
    //        allWayPoints[i + 1] = path.corners[i];
    //    }

    //    float pathLength = 0f;

    //    for (int i = 0; i < allWayPoints.Length - 1; i++)
    //    {
    //        pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
    //    }

    //    return pathLength;

    //}
    */

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.x + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, 1 << 8))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code here
            Debug.Log("Attack");
            Debug.Log(transform.forward);
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
