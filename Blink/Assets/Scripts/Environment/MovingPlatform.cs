using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Note: Points are in world space, not local space")]
    public Vector3[] Points;
    public bool WillWait = true;
    private int CurrentPointIndex = 0;
    private Vector3 CurrentTarget;
    private float TimeWaited;
    private float SnappingDistance;

    public GameObject attachedObject;

    public bool enterCalled = false;

    [SerializeField] private float Speed = 3;
    [SerializeField] private float PointWaitingTime = 1.5f;




    // Start is called before the first frame update
    void Start()
    {
        //SnappingDistance = 0.01f * Speed;
        if (Points.Length > 0)
        {
            CurrentTarget = Points[0];
        }
    }

    // Must use FixedUpdate otherwise Character Controller won't track with platform
    void FixedUpdate()
    {
        if (BlinkMgr.Instance.BlinkActive)
        {
            return;
        }
            SnappingDistance = Time.deltaTime * Speed; // Snaps if next step overshoots target
        if (Points.Length <= 0) // No points, therefore static platform
        {
            return;
        }
        if (transform.position != CurrentTarget)
        {
            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    private void MovePlatform()
    {
        Vector3 direction = (CurrentTarget - transform.position).normalized;
        transform.position += direction * Speed * Time.deltaTime;
        if (attachedObject != null)
        {
            attachedObject.transform.position += direction * Speed * Time.deltaTime;
        }

        if (IsWithinSnappingDistance())
        {
            transform.position = CurrentTarget;

        }
    }

    private bool IsWithinSnappingDistance()
    {
        return (CurrentTarget - transform.position).magnitude < SnappingDistance;
    }

    private void UpdateTarget()
    {
        if (WillWait && TimeWaited < PointWaitingTime)
        {
            TimeWaited += Time.deltaTime;
        }
        else
        {
            TimeWaited = 0;
            CurrentPointIndex = CurrentPointIndex + 1 < Points.Length ? CurrentPointIndex + 1 : 0; // Reset index if overflow
            CurrentTarget = Points[CurrentPointIndex];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            attach(other.gameObject); // Lock object onto platform
        }
    }

    public void attach(GameObject other)
    {
        attachedObject = other;
    }

    public void detach()
    {
        attachedObject = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player" && attachedObject == null)
        {
            attach(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            detach();
        }
    }
}
