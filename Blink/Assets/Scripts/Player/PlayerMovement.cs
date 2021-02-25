using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 35;
    public float counterMovement = 0.175f;
    public float sprintMultiplier = 3f;
    public float airbourneSpeed = 0.5f;

    Vector3 velocity;

    public float jumpHeight = 580f;
    private float jumpCooldown = 0.25f;
    public float slideJumpHeight = 6f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = -0.4f;

    private bool isOnSlope = false;
    private Vector3 feetPos;
    public float slopeCheckHeight = 0.5f;
    private Vector3 slopeDirection;
    public float airDragPercent = 0.1f;
    public float groundDragPercent = 0.8f;
    public float slopeDragPercent = 0f;
    private Vector3 input_movement;
    private float maxWalkSpeed = 20;
    private float maxRunSpeed = 40;
    private float maxSlideSpeed = 50;
    private float maxAirSpeed = 40;
    private float maxSpeed = 20;
    private Vector3 slopeNormal;
    [SerializeField] private float timeToVault = 2f;
    [SerializeField] private float timeToClimb = 4f;
    private float t_parkour = 0f;
    public Animator cameraAnimator;

    private float x, y;
    private bool isCrouching, isGrounded, isJumping;

    public AudioSource runningAudio; // For future iterations
    public AudioSource jumpingAudio;

    public LayerMask whatIsGround;
    [SerializeField] private Transform vaultHeight;
    [SerializeField] private Transform climbHeight;
    [SerializeField] private Camera cam;
    [SerializeField] Vector3 vaultTo;
    [SerializeField] Vector3 climbTo;
    private bool isClimbing = false, isVaulting = false;
    private Vector3 from;
    private Vector3 to;
    private float parkour_dist;

    private bool canVault = false;
    private bool canClimb = false;
    private bool canJump = true;

    private float slopeAngle;

    void Start()
    {
        Time.timeScale = 1;
        BlinkMgr.Instance.BlinkTimer = 3f;
        rb = GetComponent<Rigidbody>();
        cameraAnimator = cam.GetComponent<Animator>();
    }

    private void Update()
    {
        print(isGrounded);
        CheckSlopes();
        CheckParkourRays();
        Move();
        if (canClimb || canVault)
        {
            Parkour();
        }
    }
    private void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        isJumping = Input.GetKey(KeyCode.Space);
        Vector2 magnitude = FindVelRelativeToCam();
        float xMag, yMag;
        xMag = magnitude.x;
        yMag = magnitude.y;
        CounterMovement(xMag, yMag, magnitude);
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * 2f); // More gravity
        }
        if (isGrounded && isJumping && !isVaulting)
        {
            Jump();
        }
        if (isCrouching && isOnSlope)
        {
            rb.AddForce(slopeDirection * slopeAngle * 3);
        }
        if (isCrouching && isGrounded && canJump)
        {
            rb.AddForce(Vector3.down * 2500 * Time.deltaTime);
            return; //Skip max speed while going down ramps
        }
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        float forwardSpeedMultiplier = 1f;
        float speedMultipler = 1f;

        if (!isGrounded)
        {
            forwardSpeedMultiplier /= 2;
            speedMultipler /= 2;
        }
        if (isGrounded && isCrouching)
        {
            forwardSpeedMultiplier = 0f;
        }

        if(Input.GetKey(KeyCode.LeftShift) && StaminaBar.instance.GetCurrentStamina() > 0)
        {
            StaminaBar.instance.UseStamina(true);
            speedMultipler = speedMultipler * sprintMultiplier;
            runningAudio.enabled = true;
        }
        else
        {
            runningAudio.enabled = false;
        }
        rb.AddForce(transform.forward * y * speed * Time.deltaTime * speedMultipler * forwardSpeedMultiplier);
        rb.AddForce(transform.right * x * speed * Time.deltaTime * speedMultipler);
    }
    private void CounterMovement(float x, float y, Vector2 mag)
    {
        float threshold = 0.01f;
        if (!isGrounded) return;

        if (isCrouching)
        {
            rb.AddForce(speed * Time.deltaTime * -rb.velocity.normalized * 0.2f);
            return;
        }

        if (!(Mathf.Abs(mag.x) > threshold && Mathf.Abs(x) < 0.05f) || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(speed * transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (!(Mathf.Abs(mag.y) > threshold && Mathf.Abs(y) < 0.05f) || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(speed * transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }


    private Vector2 FindVelRelativeToCam()
    {
        float lookAngle = transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void Parkour()
    {
        if (canVault)
        {
            if (!(Input.GetKey(KeyCode.Space)))
            {
                return;
            }
            from = transform.position;
            to = transform.position + transform.TransformDirection(vaultTo);
            canVault = false;
            isVaulting = true;
            rb.isKinematic = true;
            t_parkour = 0;
        }

        if (canClimb)
        {
            if (!(Input.GetAxisRaw("Vertical") > 0))
            {
                return;
            }
            from = transform.position;
            to = transform.position + transform.TransformDirection(climbTo);
            canClimb = false;
            isClimbing = true;
            rb.isKinematic = true;
            t_parkour = 0;
        }
        while (isVaulting)
        {
            cameraAnimator.CrossFade("Vault", 0.1f);
            t_parkour += Time.deltaTime / timeToVault;
            if (t_parkour >= 1f)
            {
                isVaulting = false;
                rb.isKinematic = false;
            }
            transform.position = Vector3.Lerp(from, to, t_parkour);
        }
        while (isClimbing)
        {
            cameraAnimator.CrossFade("Climb", 0.1f);
            t_parkour += Time.deltaTime / timeToClimb;
            if (t_parkour >= 1f)
            {
                isClimbing = false;
                rb.isKinematic = false;
            }
            else
            {
                transform.position = Vector3.Lerp(from, to, t_parkour);
            }
        }
    }
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
    private void Jump()
    {
        if (canJump)
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpHeight);
            rb.AddForce(slopeNormal * jumpHeight);
            jumpingAudio.Play();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        canJump = true;
    }

    /*
     private void AirBorne()
    {
        velocity.y += gravity * Time.deltaTime;
        velocity += input_movement * airbourneSpeed;
        velocity.x = Mathf.Min(velocity.x, maxAirSpeed);
        velocity.z = Mathf.Min(velocity.z, maxAirSpeed);
    }
    */

    private void CheckSlopes()
    {
        if (!isGrounded)
        {
            isOnSlope = false;
            return;
        }
        CapsuleCollider collide = GetComponent<CapsuleCollider>();
        feetPos = transform.position - new Vector3(0, collide.height / 2, 0);
        if (Physics.Raycast(feetPos, Vector3.down, out RaycastHit slopeHit, slopeCheckHeight, whatIsGround))
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
        Debug.DrawLine(feetPos, feetPos + slopeCheckHeight * Vector3.down, Color.red);
        Debug.DrawLine(feetPos, feetPos + slopeDirection * slopeAngle, Color.magenta);
    }

    private void CheckParkourRays()
    {
        RaycastHit hit;
        RaycastHit obstructionHit;
        Vector3 dir = cam.transform.forward;
        Vector3 pos = climbHeight.position;
        Vector3 obstruction_pos = new Vector3(pos.x, pos.y + .5f, pos.z);
        dir.y = 0;
        Debug.DrawLine(pos, pos + dir * 1f, Color.red);
        if (Physics.Raycast(pos, dir, out hit, 1f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out obstructionHit, 1f, whatIsGround))
        {
            if (!isClimbing)
            {
                canClimb = true;
                return;
            }
        }
        pos = vaultHeight.position;
        obstruction_pos = new Vector3(pos.x, pos.y + .5f, pos.z);
        if (!canClimb && Physics.Raycast(pos, dir, out hit, 1f, whatIsGround) && !Physics.Raycast(obstruction_pos, dir, out obstructionHit, 1f, whatIsGround))
        {
            if (!isVaulting)
            {
                canVault = true;
                return;
            }
        }
    }
}

