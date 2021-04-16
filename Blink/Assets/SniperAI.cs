using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SniperAI : MonoBehaviour
{
    public Vector3[] Points;
    private int currentPoint;
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public Transform meleeTarget;
    public LayerMask obstacleMask;

    public GameObject player;

    public float sightRange;
    public LayerMask whatIsPlayer;

    private bool takingDamage;
    private bool targetCoverSet = false;
    private Vector3 targetCover;

    private float takingDamangeTimer = 0f;

    private bool playerSpotted = false;

    private string stage;

    private NavMeshAgent agent;

    public float scanSpeed;

    private float rotationCompleted = 0f; 
    private bool rotationSet = false;
    private Animator anim;

    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public GameObject projectile;

    public LineRenderer Laser;
    public GameObject LaserPos;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.Find("Player");
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        anim = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();

        currentPoint = 0;
        stage = "moving";
    }

    // Update is called once per frame
    void Update()
    {
        if (!BlinkMgr.Instance.BlinkActive)
        {
            if (stage == "moving")
            {
                anim.SetTrigger("Walk");
                if (Mathf.Approximately(this.transform.position.x, Points[currentPoint].x) && Mathf.Approximately(this.transform.position.z, Points[currentPoint].z) && (this.transform.position.y - Points[currentPoint].y < 1 || this.transform.position.y - Points[currentPoint].y > -1))
                {
                    stage = "scanning";

                }
                else
                {
                    agent.SetDestination(Points[currentPoint]);
                }
            }
            else if (stage == "scanning")
            {
                anim.SetTrigger("Idle");
                if (!rotationSet)
                {
                    rotationCompleted = 0f;
                    rotationSet = true;
                }
                float rotationAmount = scanSpeed * Time.deltaTime;
                this.transform.Rotate(new Vector3(0f, 1f, 0f) * rotationAmount);
                rotationCompleted += rotationAmount;
                if (rotationCompleted >= 360f)
                {
                    stage = "moving";
                    rotationSet = false;
                    incrementPoint();

                }
            }
            else if (stage == "attacking")
            {
                rotationSet = false;
                if (isPlayerInSight())
                {
                    Laser.enabled = true;
                    Vector3[] pos = { LaserPos.transform.position, player.transform.position };
                    Laser.SetPositions(pos);
                    anim.SetTrigger("Idle");
                    transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
                    agent.transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
                    StartCoroutine("attack");
                } else
                {
                    Laser.enabled = false;
                    stage = "scanning";
                }
            }
            bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            if (playerInSightRange && stage != "attacking")
            {
                Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, whatIsPlayer);
                for (int i = 0; i < targetsInViewRadius.Length; i++)
                {
                    Transform target = targetsInViewRadius[i].transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                    {
                        float dToTarget = Vector3.Distance(transform.position, target.position);
                        if (!Physics.Raycast(transform.position, dirToTarget, dToTarget, obstacleMask) && isPlayerInSight()) {
                            agent.SetDestination(transform.position);
                            stage = "attacking";
                        }
                    }
                }

            }
        }
    }

    void incrementPoint()
    {
        currentPoint += 1;
        if (currentPoint == Points.Length)
        {
            currentPoint = 0;
        }
    }

    public bool isPlayerInSight()
    {
        float maxDist = Vector3.Distance(LaserPos.transform.position, player.transform.position);
        Vector3 dir = player.transform.position - LaserPos.transform.position;
        Ray r = new Ray(LaserPos.transform.position, dir);
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


    IEnumerator attack()
    {
        if (!alreadyAttacked)
        {
            // Attack code here
            alreadyAttacked = true;
            Vector3 aim = (player.transform.position - LaserPos.transform.position).normalized;
            GameObject bulletObject = Instantiate(projectile);
            bulletObject.transform.rotation = projectile.transform.rotation;
            bulletObject.transform.position = LaserPos.transform.position + aim;
            bulletObject.transform.forward = aim;
            yield return new WaitForSeconds(timeBetweenAttacks);
            alreadyAttacked = false;
            
        }
    }
}
