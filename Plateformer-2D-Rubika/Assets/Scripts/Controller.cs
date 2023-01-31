using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Hold all the parameters for the controller like speed, gravity, etc.
    [SerializeField] ControllerData data;

    Rigidbody2D rb;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public LayerMask groundLayer;

    //All timer
    float lastGroundTime;
    float lastJumpPressed;

    //All bool
    bool isJumping;
    bool isFalling;
    bool jumpCut;

    //All Vector2
    Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        SetGravityScale(data.gravityScale);
    }

    private void Update()
    {
        lastGroundTime -= Time.deltaTime;
        lastJumpPressed -= Time.deltaTime;

        //Check if player is grounded
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer) && !isJumping)
            lastGroundTime = data.coyoteTime;


        MyInputs();
        GravityCheck();
        JumpCheck();
    }

    void JumpCheck()
    {
        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
            isFalling = true;
        }
        if (lastGroundTime > 0 && !isJumping)
        {
            jumpCut = false;
            isFalling = false;
        }

        if (CanJump() && lastJumpPressed > 0)
        {
            isJumping = true;
            jumpCut = false;
            isFalling = false;
            Jump();
        }

    }

    void GravityCheck()
    {
        if (rb.velocity.y < 0 && moveInput.y < 0)
        {
            SetGravityScale(data.gravityScale * data.fastFallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else if (jumpCut)
        {
            SetGravityScale(data.gravityScale * data.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else if ((isJumping || isFalling) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold)
        {
            SetGravityScale(data.gravityScale * data.jumpHangGravityMult);
        }
        else if (rb.velocity.y < 0)
        {
            SetGravityScale(data.gravityScale * data.fallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else
        {
            SetGravityScale(data.gravityScale);
        }

    }

    void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    void MyInputs()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && lastGroundTime > 0)
            lastJumpPressed = data.jumpInputBufferTime;

        if (Input.GetButtonUp("Jump") && lastGroundTime < 0)
            jumpCut = true;
    }

    void Jump()
    {
        lastJumpPressed = 0;
        lastGroundTime = 0;

        float force = data.jumpForce;
        if (rb.velocity.y < 1)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

    }

    private void FixedUpdate()
    {
        PlayerRun();
    }

    void PlayerRun()
    {
        float targetSpeed = moveInput.x * data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, 1);

        float accelRate;

        if (lastGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

        if ((isJumping || isFalling) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold)
        {
            accelRate *= data.jumpHangAccelerationMult;
            targetSpeed *= data.jumpHangMaxSpeedMult;
        }

        float speedDif = targetSpeed - rb.velocity.x;

        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }


    private bool CanJump()
    {
        return lastGroundTime > 0 && !isJumping;
    }


}
