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
