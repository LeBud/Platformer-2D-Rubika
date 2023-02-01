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

    [Header("Wall Jump Checks")]
    [SerializeField] Transform frontWallCheckPos;
    [SerializeField] Transform backWallCheckPos;

    //All timer
    float lastGroundTime;
    float lastJumpPressed;
    float lastOnWallTime;
    float lastOnWallTimeRight;
    float lastOnWallTimeLeft;

    //All bool
    bool isJumping;
    bool isFalling;
    bool jumpCut;
    bool isWallJumping;
    bool isSliding;
    bool isFacingRight;

    //Wall Jump variables
    float wallJumpStartTime;
    int lastWallJumpDir;

    //All Vector2
    Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    Vector2 wallCheckSize = new Vector2(0.5f, 1f);
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
        lastOnWallTime -= Time.deltaTime;
        lastOnWallTimeRight -= Time.deltaTime;
        lastOnWallTimeLeft -= Time.deltaTime;

        #region Check Methods
        //Check if player is grounded
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer) && !isJumping)
            lastGroundTime = data.coyoteTime;

        //Check for Walls
        //Player Facing Right
        if (((Physics2D.OverlapBox(frontWallCheckPos.position, wallCheckSize, 0, groundLayer) && isFacingRight)
                || (Physics2D.OverlapBox(backWallCheckPos.position, wallCheckSize, 0, groundLayer) && !isFacingRight)) && !isWallJumping)
            lastOnWallTimeRight = data.coyoteTime;

        //Player Facing Left
        if (((Physics2D.OverlapBox(frontWallCheckPos.position, wallCheckSize, 0, groundLayer) && !isFacingRight)
            || (Physics2D.OverlapBox(backWallCheckPos.position, wallCheckSize, 0, groundLayer) && isFacingRight)) && !isWallJumping)
            lastOnWallTimeLeft = data.coyoteTime;

        lastOnWallTime = Mathf.Max(lastOnWallTimeLeft, lastOnWallTimeRight);
        #endregion

        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);

        MyInputs();
        GravityCheck();
        JumpCheck();
        SlideCheck();
    }

    void JumpCheck()
    {
        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
            if (!isWallJumping)
                isFalling = true;
        }

        if (isWallJumping && Time.time - wallJumpStartTime > data.wallJumpTime)
        {
            isWallJumping = false;
        }

        if (lastGroundTime > 0 && !isJumping && !isWallJumping)
        {
            jumpCut = false;
            if (!isJumping)
                isFalling = false;
        }

        if (CanJump() && lastJumpPressed > 0)
        {
            isJumping = true;
            isWallJumping = false;
            jumpCut = false;
            isFalling = false;
            Jump();
        }
        else if (CanWallJump() && lastJumpPressed > 0)
        {
            isWallJumping = true;
            isJumping = false;
            jumpCut = false;
            isFalling = false;
            wallJumpStartTime = Time.time;
            lastWallJumpDir = (lastOnWallTimeRight > 0) ? -1 : 1;

            WallJump(lastWallJumpDir);
        }
    }

    void SlideCheck()
    {
        if (CanSlide() && ((lastOnWallTimeLeft > 0 && moveInput.x < 0) || (lastOnWallTimeRight > 0 && moveInput.x > 0)))
            isSliding = true;
        else
            isSliding = false;

    }

    void GravityCheck()
    {
        if (isSliding)
        {
            SetGravityScale(0);
        }
        else if (rb.velocity.y < 0 && moveInput.y < 0)
        {
            SetGravityScale(data.gravityScale * data.fastFallGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else if (jumpCut)
        {
            SetGravityScale(data.gravityScale * data.jumpCutGravityMult);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
        }
        else if ((isJumping || isFalling || isWallJumping) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold)
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
            if (CanWallJumpCut() || CanJumpCut())
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

    void WallJump(int dir)
    {
        lastJumpPressed = 0;
        lastGroundTime = 0;
        lastOnWallTimeRight = 0;
        lastOnWallTimeLeft = 0;

        Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);

        if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
            force.x -= rb.velocity.x;

        if (rb.velocity.y < 0)
            force.y -= rb.velocity.y;

        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (isWallJumping)
            PlayerRun(data.wallJumpRunLerp);
        else
            PlayerRun(1);

        if (isSliding)
            Slide();
    }

    void PlayerRun(float lerpAmount)
    {
        float targetSpeed = moveInput.x * data.runMaxSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

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

    void Slide()
    {
        float speedDif = data.slideSpeed - rb.velocity.y;
        float movement = speedDif * data.slideAccel;

        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        rb.AddForce(movement * Vector2.up);
    }

    bool CanJump()
    {
        return lastGroundTime > 0 && !isJumping;
    }

    bool CanWallJump()
    {
        return lastJumpPressed > 0 && lastOnWallTime > 0 && lastGroundTime <= 0 && (!isWallJumping ||
             (lastOnWallTimeRight > 0 && lastWallJumpDir == 1) || (lastOnWallTimeLeft > 0 && lastWallJumpDir == -1));
    }

    bool CanSlide()
    {
        if (lastOnWallTime > 0 && !isJumping && !isWallJumping && lastGroundTime <= 0)
            return true;
        else
            return false;
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
            Turn();
    }

    void Turn()
    {
        //stores scale and flips the player along the x axis, 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }

    private bool CanJumpCut()
    {
        return isJumping && rb.velocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return isWallJumping && rb.velocity.y > 0;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(frontWallCheckPos.position, wallCheckSize);
        Gizmos.DrawWireCube(backWallCheckPos.position, wallCheckSize);

    }
}
