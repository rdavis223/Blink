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

    void Start()
    {
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
            rb.AddRelativeForce(targetVel * airSpeed * Time.deltaTime);
        }
        
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
        if (Input.GetKey(KeyCode.V)){
            rb.AddRelativeForce(new Vector3(0, 0, 4000));
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
        Debug.DrawLine(feetPos, feetPos + Vector3.down * groundDistance, Color.yellow);
        Debug.DrawLine(feetPos, feetPos + slopeDirection * slopeAngle, Color.magenta);
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

    /*
        private void CounterMovement(Vector3 axis)
        {
            float drag = 200f;
            if (!isGrounded) drag = 100f;
            else if (isCrouching) drag = 10f;
            Vector3 counterForce = rb.velocity * -1 * drag * Time.deltaTime;
            counterForce.x *= Mathf.Abs(axis.x);
            counterForce.y *= Mathf.Abs(axis.y);
            counterForce.z *= Mathf.Abs(axis.z);
            rb.AddForce(counterForce);
        }
    */

    /*
    private Vector2 FindVelRelativeToCam()
    {
        float lookAngle = transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }
    */


    /*
        private void Walk()
        {
            velocity += input_movement * speed;
            velocity = Vector3.ClampMagnitude(velocity, maxWalkSpeed);
            if (isOnSlope)
            {
                velocity.y = -2;
            }
        }
        private void Run()
        {
            velocity += input_movement * sprintSpeed;
            velocity = Vector3.ClampMagnitude(velocity, maxRunSpeed);
        }

        private void Slide()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                velocity = slopeNormal * slideJumpHeight;
                jumpingAudio.Play();
            }
            else
            {
                velocity += slopeDirection * slopeAngle * 20;
                velocity = Vector3.ClampMagnitude(velocity, maxSlideSpeed);
            }
        }
    */



    /*
     private void AirBorne()
    {
        velocity.y += gravity * Time.deltaTime;
        velocity += input_movement * airbourneSpeed;
        velocity.x = Mathf.Min(velocity.x, maxAirSpeed);
        velocity.z = Mathf.Min(velocity.z, maxAirSpeed);
    }
    */
}

