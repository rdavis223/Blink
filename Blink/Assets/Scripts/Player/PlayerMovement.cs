using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float airSpeed;
    public float gravity;
    public float sprintMultiplier;

    Vector3 velocity;

    public float jumpHeight;
    public float jumpCooldown;

    public Transform groundCheck;
    public float groundDistance;

    [Header("Step settings")]
    [SerializeField] private GameObject stepRayLower;
    [SerializeField] private GameObject stepRayHigher;
    [SerializeField] private float stepHeight; // Controls max height player can climb
    [SerializeField] private float stepSmooth; // The incremented height increase applied each FixedUpdate - higher climbs faster

    [Header("Parkour settings")]
    private bool isOnSlope = false;
    private Vector3 feetPos;
    private Vector3 slopeDirection;
    private float maxVelocityChange = 10f;
    private Vector3 slopeNormal;
    [SerializeField] private float timeToVault;
    [SerializeField] private float timeToClimb;
    private float t_parkour = 0f;
    // public Animator animator;

    private float x, z;
    private bool isCrouching, isGrounded, isJumping, isSprinting;

    public AudioSource runningAudio; // For future iterations
    public AudioSource jumpingAudio;

    public LayerMask whatIsGround;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform vaultHeight;
    [SerializeField] private Transform climbHeight;
    [SerializeField] Vector3 vaultTo;
    [SerializeField] Vector3 climbTo;
    private bool isClimbing = false, isVaulting = false;
    private Vector3 from;
    private Vector3 to;
    private float parkour_dist;

    private bool canVault = false;
    private bool canClimb = false;
    private bool canJump = true;
    private bool movementOverride = false;

    private float slopeAngle;

    void Start()
    {
        Time.timeScale = 1;
        BlinkMgr.Instance.BlinkTimer = 3f;
        rb = GetComponent<Rigidbody>();

        Vector3 pos = stepRayHigher.transform.position;
        stepRayHigher.transform.position = new Vector3(pos.x, pos.y + stepHeight, pos.z);  // Offset raycast by height
    }

    private void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        isJumping = Input.GetKey(KeyCode.Space);
        CheckSlopes();
        CheckParkourRays();
        Move();
        StepClimb();
        //Parkour();
        Slide();
        Jump();
    }
    private void Move()
    {
        // Ignore inputs if vaulting, climbing, sliding, or anything else added
        if (movementOverride) return;

        var targetVel = new Vector3(x, 0, z);
        targetVel = transform.TransformDirection(targetVel);
        targetVel *= speed;
        if (isSprinting && z > 0) // Only apply sprint when moving forward, not back or strafing
        {
            targetVel.z *= sprintMultiplier;
        }
        var deltaVel = targetVel - rb.velocity;
        deltaVel.x = Mathf.Clamp(deltaVel.x, -maxVelocityChange, maxVelocityChange);
        deltaVel.z = Mathf.Clamp(deltaVel.z, -maxVelocityChange, maxVelocityChange);
        deltaVel.y = 0;
        if (isGrounded)
        {
            rb.AddForce(deltaVel, ForceMode.VelocityChange);
        }
        else
        {
            rb.AddForce(targetVel * airSpeed * Time.deltaTime);
        }

        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
    }

    private void StepClimb()
    {
        Vector3 forward_dir = transform.TransformDirection(Vector3.forward);
        Vector3 left_dir_45 = transform.TransformDirection(1.5f, 0, 1); // Values arrived via pythagorus theorem for a 45 degree triangle
        Vector3 right_dir_45 = transform.TransformDirection(-1.5f, 0, 1);
        float topDist = 0.7f;
        float bottomDist = 0.8f;
        bool isOnStep = false;

        Debug.DrawLine(stepRayLower.transform.position, stepRayLower.transform.position + forward_dir * topDist, Color.cyan);
        Debug.DrawLine(stepRayHigher.transform.position, stepRayHigher.transform.position + forward_dir * bottomDist, Color.magenta);
        if (x == 0 && z == 0)
        {
            return;
        }

        RaycastHit lowHit;
        RaycastHit highHit;


        // Forward cast
        if (Physics.Raycast(stepRayLower.transform.position, forward_dir, out lowHit, topDist, whatIsGround))
        {
            if (!Physics.Raycast(stepRayHigher.transform.position, forward_dir, out highHit, bottomDist, whatIsGround))
            {
                isOnStep = true;
            }
        }

        // Left side cast at 45 degrees
        if (Physics.Raycast(stepRayLower.transform.position, left_dir_45, out lowHit, topDist, whatIsGround))
        {
            if (!Physics.Raycast(stepRayHigher.transform.position, left_dir_45, out highHit, bottomDist, whatIsGround))
            {
                isOnStep = true;
            }
        }

        // Right side cast at 45 degrees
        if (Physics.Raycast(stepRayLower.transform.position, right_dir_45, out lowHit, topDist, whatIsGround))
        {
            if (!Physics.Raycast(stepRayHigher.transform.position, right_dir_45, out highHit, bottomDist, whatIsGround))
            {
                isOnStep = true;
            }
        }

        if (isOnStep)
        {
            rb.velocity = new Vector3(rb.velocity.x, 2, rb.velocity.z);  // Seems to even out movement - seems to jitter up and down otherwise
            rb.position += new Vector3(0f, stepSmooth, 0f);
        }


    }

    private void Slide()
    {
        if (isCrouching && isOnSlope)
        {

            rb.AddRelativeForce(Vector3.forward * 4);
            rb.AddForce(new Vector3(0, -10, 0)); // Force player onto ground
            rb.AddForce(slopeDirection * Mathf.Sqrt(slopeAngle) * 50);
            movementOverride = true;
        }
        else
        {
            movementOverride = false;
        }
    }

    private void Jump()
    {
        if (isJumping && isGrounded && canJump)
        {
            canJump = false;
            //Vector3 jumpForce = (Mathf.Sqrt(2 * jumpHeight * gravity)) * slopeNormal;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Remove all negative y acceleration from sliding
            Vector3 jumpForce = jumpHeight * slopeNormal;
            rb.AddForce(jumpForce, ForceMode.Acceleration);
            jumpingAudio.Play();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Parkour()
    {
        if (canVault && isJumping)
        {
            from = transform.position;
            to = transform.position + transform.TransformDirection(vaultTo);
            canVault = false;
            isVaulting = true;
            rb.isKinematic = true;
            movementOverride = true;
            t_parkour = 0;
        }

        if (canClimb && x > 0)
        {
            from = transform.position;
            to = transform.position + transform.TransformDirection(climbTo);
            canClimb = false;
            isClimbing = true;
            rb.isKinematic = true;
            movementOverride = true;
            t_parkour = 0;
        }
        while (isVaulting)
        {
            //animator.CrossFade("Vault", 0.1f);
            t_parkour += Time.deltaTime / timeToVault;
            if (t_parkour >= 1f)
            {
                isVaulting = false;
                rb.isKinematic = false;
                movementOverride = false;
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            transform.position = Vector3.Lerp(from, to, t_parkour);
        }
        while (isClimbing)
        {
            print("climbing");
            //animator.CrossFade("Climb", 0.1f);
            t_parkour += Time.deltaTime / timeToClimb;
            if (t_parkour >= 1f)
            {
                isClimbing = false;
                rb.isKinematic = false;
                movementOverride = false;
                Invoke(nameof(ResetJump), jumpCooldown);
            }
            else
            {
                transform.position = Vector3.Lerp(from, to, t_parkour);
            }
        }
    }

    private void CheckSlopes()
    {
        if (!isGrounded)
        {
            isOnSlope = false;
            return;
        }
        CapsuleCollider collide = GetComponent<CapsuleCollider>();
        feetPos = transform.position - new Vector3(0, collide.height / 2, 0);
        feetPos.y += 0.05f;
        if (Physics.Raycast(feetPos, Vector3.down, out RaycastHit slopeHit, groundDistance, whatIsGround))
        {
            slopeNormal = slopeHit.normal;
            slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            if (slopeAngle == 0)
            {
                isOnSlope = false;
                return;
            }
            else
            {
                isOnSlope = true;
                Vector3 temp = Vector3.Cross(slopeHit.normal, Vector3.down);
                slopeDirection = Vector3.Cross(temp, slopeHit.normal);
            }
        }
        else
        {
            isOnSlope = false;
            return;
        }
    }

    private void CheckParkourRays()
    {
        Vector3 dir = cam.transform.forward;
        Vector3 pos = climbHeight.position;
        Vector3 obstruction_pos = new Vector3(pos.x, pos.y + .4f, pos.z);
        dir.y = 0;
        if (Physics.Raycast(pos, dir, out RaycastHit hit, 0.65f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out RaycastHit obstructionHit, 0.65f, whatIsGround))
        {
            if (!isClimbing)
            {
                canClimb = true;
            }
        }
        pos = vaultHeight.position;
        obstruction_pos = new Vector3(pos.x, pos.y + .4f, pos.z);
        if (!canClimb && Physics.Raycast(pos, dir, out hit, 0.65f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out obstructionHit, 0.65f, whatIsGround))
        {
            if (!isVaulting)
            {
                canVault = true;
            }
        }
    }
}