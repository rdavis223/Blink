using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform[] points;
    public GameObject destination;
    private NavMeshAgent agent;
    private int destPoint = 0;
    public GameObject playerModel;

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //public Transform player;
    //public LayerMask whatIsGround, whatIsPlayer;
    //public float health;

    //// Patrolling
    //public Vector3 walkPoint;
    //bool walkPointSet;
    //public float walkPointRange;

    //// Attacking
    //public float timeBetweenAttacks;
    //bool alreadyAttacked;
    //public GameObject projectile;

    //// STates
    //public float sightRange, attackRange;
    //public bool playerInSightRange, playerInAttackRange;

    //private void Awake()
    //{
    //    player = GameObject.Find("PlayerController").transform;
    //    agent = GetComponent<NavMeshAgent>();
    //}

    //private void Update()
    //{
    //    // Check for sight and attack range
    //    playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
    //    playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    //    if (!playerInSightRange && !playerInAttackRange) Patrolling();
    //    if (playerInSightRange && !playerInAttackRange) ChasePlayer();
    //    if (playerInSightRange && playerInAttackRange) AttackPlayer();
    //}

    //private void Patrolling()
    //{
    //    if (!walkPoint) SearchWalkPoint();

    //    if (walkPointSet)
    //        agent.SetDestination(walkPoint);

    //    Vector3 distanceToWalkPoint = transform.position - walkPoint;

    //    // Walkpoint reached
    //    if (distanceToWalkPoint.magnitude < 1f)
    //        walkPointSet = false;
    //}

    //private void SearchWalkPoint()
    //{
    //    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float randomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.x + randomZ);

    //    if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
    //        walkPointSet = true;
    //}

    //private void ChasePlayer()
    //{
    //    agent.SetDestination(player.position);
    //}

    //private void AttackPlayer()
    //{
    //    agent.SetDestination(transform.position);

    //    transform.LookAt(player);

    //    if (!alreadyAttacked)
    //    {
    //        //Attack code here
    //        Rigidbody rb = Instantiate(projectile, tranform.position, Quaternion.identity).GetComponent<RigidBody>();
    //        rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
    //        rb.AddForce(tranform.up * 8f, ForceMode.Impulse);
    //        alreadyAttacked = true;
    //        invoke(nameof(ResetAttack), timeBetweenAttacks);
    //    }
    //}

    //private void ResetAttack()
    //{
    //    alreadyAttacked = false;
    //}

    //public void TakeDamage(int damage)
    //{
    //    health -= damage;

    //    if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    //}

    //private void DestroyEnemy()
    //{
    //    Destroy(gameObject);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWirteSphere(transform.position, attackRange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, sightRange);
    //}
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; // continuous movement between points
        GoToNextPoint();
    }

    void GoToNextPoint() {
        if (points.Length == 0) {
            return;
        }

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void Update()
    {

        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            GoToNextPoint();
        }
    }
}
