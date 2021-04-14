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
    [SerializeField] private float slopeSpeed;

    void Start()
    {
        // May be possible to delete these lines? - Not relevent to movement
        Time.timeScale = 1;
        BlinkMgr.Instance.BlinkTimer = 3f;

        rb = GetComponent<Rigidbody>();
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
        Parkour();
        Slide();
        Jump();
    }
    private void Move()
    {
        // Ignore inputs if vaulting, climbing, sliding, or anything else added
        if (movementOverride) return;

        var targetVel = new Vector3(x, 0, z);
        targetVel *= speed;
        targetVel = transform.TransformDirection(targetVel); // Make movement relative to camera (a = left for example) - movement is in world space otherwise
        if (isSprinting && z > 0) // Only apply sprint when moving forward, not back or strafing
        {
            targetVel.z *= sprintMultiplier;
        }
        var deltaVel = targetVel - rb.velocity;  // deltaVel holds the amount to increase/decrease - used in Impulse force mode
        deltaVel.x = Mathf.Clamp(deltaVel.x, -maxVelocityChange, maxVelocityChange);
        deltaVel.z = Mathf.Clamp(deltaVel.z, -maxVelocityChange, maxVelocityChange);
        deltaVel.y = 0; // y is handled in Jump()
        if (isGrounded)
        {
            // Impulse mode gives CharacterController-esque movement - resets force to 0 after moving
            // Momentum is not maintained
            rb.AddForce(deltaVel, ForceMode.Impulse); 
        }
        else
        {
            // Air speed must maintain momentum - notice targetVel instead of deltaVel - we are not using Impulse force mode
            rb.AddForce(targetVel * airSpeed * Time.deltaTime);
        }

        // Apply gravity
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
    }

    private void Slide()
    {
        if (isCrouching && isOnSlope)
        {
            rb.AddRelativeForce(Vector3.forward * 4);  // Apply a slight speed boost - needs tweaking - see Titanfall 2 for example
            rb.AddForce(new Vector3(0, -10, 0)); // Force player onto ground on slopes
            // Sqrt seemed to help manage steep slopes accelerating too much - needs tweaking
            rb.AddForce(slopeDirection * Mathf.Sqrt(slopeAngle) * slopeSpeed);
            movementOverride = true; // Lock player into slide forward (cannot move side to side) - again see Titanfall 2 for example
        }
        else
        {
            // POSSIBLE BUG? - May overwrite another function's assignment to movement override
            movementOverride = false;  // Player is no longer sliding
        }
    }

    private void Jump()
    {
        if (isJumping && isGrounded && canJump)
        {
            canJump = false;  // Do not allow double jumps
            // Remove all negative y acceleration from sliding else all jumps on slopes are small
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); 
            Vector3 jumpForce = jumpHeight * slopeNormal;  // Jump off ground, possibly at an angle - Newton's 3rd Law
            rb.AddForce(jumpForce, ForceMode.Impulse);
            jumpingAudio.Play();
            Invoke(nameof(ResetJump), jumpCooldown);  // Cooldown stops multiple jump inputs while ground check is still true
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    // Still under construction
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
        // Debug.DrawLine(feetPos, feetPos + Vector3.down * groundDistance, Color.yellow);
        // Debug.DrawLine(feetPos, feetPos + slopeDirection * slopeAngle, Color.magenta);
        CapsuleCollider collide = GetComponent<CapsuleCollider>(); // Assumes the capsule collider covers the entire height - may need to be hardcoded in after avatar
        feetPos = transform.position - new Vector3(0, collide.height / 2, 0);  // Get point that is 1/2 height from the midpoint - ie the bottom-most position
        feetPos.y += 0.05f;  // Raise feet pos by a small amount as it can fail on slopes - feetpos can clip through some ground
        
        // Raycast to ground
        if (Physics.Raycast(feetPos, Vector3.down, out RaycastHit slopeHit, groundDistance, whatIsGround))
        {
            slopeNormal = slopeHit.normal;
            slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
            if (slopeAngle == 0)  // Slope normal and Vector3.up are the same - not a slope
            {
                isOnSlope = false;
                return;
            }
            else
            {
                isOnSlope = true;
                // Gets a line that crosses the slope left to right along it's slope angle
                Vector3 temp = Vector3.Cross(slopeHit.normal, Vector3.down);
                // Get perpendicular line between the line cutting across the slope (X) agaisnt the normal (Y) to get the line going down the slope (Z)
                // Recall the right hand rule, but instead of "in" or "out" of the paper, we are finding the forward vector (ie. your index finger)
                slopeDirection = Vector3.Cross(temp, slopeHit.normal); 
            }
        }
        else
        {
            isOnSlope = false;
            return;
        }
    }

    // Under construction
    private void CheckParkourRays()
    {
        Vector3 dir = cam.transform.forward;
        Vector3 pos = climbHeight.position;
        Vector3 obstruction_pos = new Vector3(pos.x, pos.y + .4f, pos.z);
        dir.y = 0;
        Debug.DrawLine(pos, pos + dir * 0.65f, Color.red);
        if (Physics.Raycast(pos, dir, out RaycastHit hit, 0.65f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out RaycastHit obstructionHit, 0.65f, whatIsGround))
        {
            if (!isClimbing)
            {
                canClimb = true;
            }
        }
        pos = vaultHeight.position;
        obstruction_pos = new Vector3(pos.x, pos.y + .4f, pos.z);
        Debug.DrawLine(pos, pos + dir * 0.65f, Color.red);
        if (!canClimb && Physics.Raycast(pos, dir, out hit, 0.65f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out obstructionHit, 0.65f, whatIsGround))
        {
            if (!isVaulting)
            {
                canVault = true;
            }
        }
    }
}