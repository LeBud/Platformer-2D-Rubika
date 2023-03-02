using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    AirFlow airFlow;

    [Header("Controller Data")]
    public PlayerControllerData playerControllerData;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundCheckLayerMask;

    [Header("AirFlow")]
    [SerializeField] LayerMask airFlowLayerMask;

    [Header("Slow Platform")]
    [SerializeField] LayerMask slowPlatformLayerMask;
    [SerializeField] float slowMultMovement;
    [SerializeField] float slowMultJump;

    [Header("Clamp Velocity")]
    [SerializeField] float maxVelocity;

    [Header("Can Glide")]
    public bool canGlide;

    public int deathCounter;

    bool jumpCut;
    bool isJumping;
    [HideInInspector]
    public bool gliding;
    bool glideJump;
    bool glideSpeed;
    bool isFlying;
    bool flyRequierement;
    bool airFlowing;
    bool onSlowPlatform;
    bool slowMov;

    float lastPressedJump;
    float onGround;
    float airFlowForce;

    [HideInInspector]
    public float glideTime;
    [HideInInspector]
    public float flyTime;

    Vector2 moveInput;

    //CheckPoint settings
    [HideInInspector]
    public Vector2 checkPointPos;
    [HideInInspector]
    public int currentCheckPoint = 0;

    //AirFlow
    [HideInInspector]
    public bool inAirFlow;
    [HideInInspector]
    public Vector2 airFlowDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        checkPointPos = transform.position;
    }

    private void Update()
    {
        //Timers
        onGround -= Time.deltaTime;
        lastPressedJump -= Time.deltaTime;
        if (gliding && !inAirFlow && rb.velocity.y < 0) glideTime -= Time.deltaTime;
        if (isFlying) flyTime -= Time.deltaTime;

        //ClampVelocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        MyInputs();
        CheckMethods();

        if (gliding && !inAirFlow && rb.velocity.y < 0) Glide();
    }

    void MyInputs()
    {
        //Movements Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");


        //Fly Input
        if (Input.GetButton("Fire1") && CanFly())
            flyRequierement = true;
        else
            flyRequierement = false;


        //Jumps Inputs
        if (Input.GetButtonDown("Jump")) 
            lastPressedJump = playerControllerData.jumpBuffer;

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) jumpCut = true;

        //AirFlow Inputs
        if (Input.GetButton("Jump") && inAirFlow && gliding)
        {
            airFlowing = true;
        }
        else airFlowing = false;

        #region GlideInputs
        //Glide Inputs
        if (!canGlide) return;
        if (playerControllerData.canGlideJump)
        {
            if (Input.GetButtonDown("Jump") && CanJumpGlide())
                    GlideJump();
        }

        if (playerControllerData.holdBtt)
        {
            if (Input.GetButton("Jump") && CanGlide())
                gliding = true;
            else if ((Input.GetButtonUp("Jump") && gliding) || glideTime <= 0)
                gliding = false;
        }
        /*else
        {

            if (Input.GetButtonDown("Jump") && CanGlide())
                gliding = true;
            else if ((Input.GetButtonUp("Jump") && gliding) || glideTime <= 0)
                gliding = false;
        }*/
        #endregion
    }

    void CheckMethods()
    {

        #region Jump
        if (!isJumping)
        {
            if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckLayerMask))
            {
                onGround = playerControllerData.coyoteTime;
                jumpCut = false;
                glideTime = playerControllerData.maxGlideTime;
                glideJump = true;
            }
        }

        if (CanJump()) Jump();

        if (rb.velocity.y <= 0 && isJumping) isJumping = false;

        if (jumpCut && isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / playerControllerData.jumpCutForce);
            jumpCut = false;
        }
        #endregion

        #region AirFlow
        if (onGround < 0)
        {
            if (Physics2D.OverlapBox(transform.position, transform.localScale, 0, airFlowLayerMask))
            {
                inAirFlow = true;
                airFlow = Physics2D.OverlapBox(transform.position, transform.localScale, 0, airFlowLayerMask).GetComponent<AirFlow>();
            }
            else
                inAirFlow = false;
        }
        else
            inAirFlow = false;

        if(airFlow != null)
        {
            airFlowDir = airFlow.dir;
            airFlowForce = airFlow.airFlowForce;
        }

        if (airFlowing) glideTime = playerControllerData.maxGlideTime;
        #endregion

        #region SlowPlatform
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, slowPlatformLayerMask))
            onSlowPlatform = true;
        else
            onSlowPlatform = false;

        #endregion

        #region Glide
        if (!playerControllerData.canGlideJump)
            glideJump = false;

        if (glideTime < playerControllerData.maxGlideTime && onGround < 0)
            glideSpeed = true;
        else
            glideSpeed = false;
        #endregion

        #region Fly
        if (flyTime >= 0 && flyRequierement) isFlying = true;
        else isFlying = false;

        if (flyTime > playerControllerData.flyMaxTime) flyTime = playerControllerData.flyMaxTime;
        #endregion
    }

    void Jump()
    {
        onGround = 0;
        lastPressedJump = 0;
        jumpCut = false;
        isJumping = true;

        float force = playerControllerData.jumpForce;

        if (onSlowPlatform)
            force = force * slowMultJump;

        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        if(rb.velocity.y > 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void Glide()
    {
        while (glideTime <= playerControllerData.maxGlideTime)
        {
            if (!gliding || inAirFlow)
                break;

            float strenght = playerControllerData.glideCurve.Evaluate(glideTime / playerControllerData.maxGlideTime);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -strenght * playerControllerData.curveMult, playerControllerData.maxGlideTime));
            return;
        }
    }

    void GlideJump()
    {
        glideJump = false;

        float upForce = playerControllerData.glideJumpForce;
        if (rb.velocity.y < 0)
            upForce -= rb.velocity.y;

        rb.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
    }
    
    private void FixedUpdate()
    {
        //Movement
        if (airFlowing)
            AirFlowMovement();
        else if (flyRequierement && flyTime > 0)
            Fly();
        else
            Movement();

        //Set gravity
        if (airFlowing)
            SetGravityScale(playerControllerData.airFlowGravity);
        else if (isFlying)
            SetGravityScale(playerControllerData.flyGravity);
        else
            SetGravityScale(playerControllerData.normalGravitysScale);
    }

    void SetGravityScale(float gravity)
    {
        rb.gravityScale = gravity;
    }

    void Movement()
    {
        float speedForce;

        if (glideSpeed) speedForce = playerControllerData.speed * playerControllerData.glideSpeedMult;
        else speedForce = playerControllerData.speed;

        if (onSlowPlatform)
            speedForce = speedForce * slowMultMovement;

        var movement = moveInput.x * speedForce;

        float acceleration = Mathf.Abs(movement) > .01f ? playerControllerData.accel : playerControllerData.deccel;
        
        movement = movement - rb.velocity.x;

        var force = new Vector2(movement * acceleration, rb.velocity.y);

        rb.AddForce(force, ForceMode2D.Force);

    }

    void AirFlowMovement()
    {

        rb.velocity = new Vector2(
            Mathf.Lerp(rb.velocity.x, airFlowDir.x * airFlowForce, playerControllerData.airFlowLerpSpeed * Time.fixedDeltaTime),
            Mathf.Lerp(rb.velocity.y, airFlowDir.y * airFlowForce, playerControllerData.airFlowLerpSpeed * Time.fixedDeltaTime));

        rb.AddForce(airFlowDir.normalized * airFlowForce, ForceMode2D.Force);
    }

    void Fly()
    {
        float speedForce = playerControllerData.speed * playerControllerData.flySpeedMult;

        var horMov = moveInput.x * speedForce;
        var verMov = moveInput.y * speedForce;

        float acceleration = Mathf.Abs(horMov) > .01f || Mathf.Abs(verMov) > .01f ? playerControllerData.accel : playerControllerData.deccel;

        horMov = horMov - rb.velocity.x;
        verMov = verMov - rb.velocity.y;

        var force = new Vector2(horMov * acceleration, verMov * acceleration);

        rb.AddForce(force, ForceMode2D.Force);
    }

    bool CanJumpGlide()
    {
        return (!isJumping || rb.velocity.y < 12) && !isFlying && glideJump && onGround < 0;
    }

    bool CanJump()
    {
        return lastPressedJump > 0 && onGround > 0 && !isFlying && !isJumping && !airFlowing;
    }

    bool CanGlide()
    {
        return !isJumping && onGround < 0 && glideTime > 0 && !isFlying && !glideJump && rb.velocity.y <= 0;
    }

    bool CanFly()
    {
        return flyTime > 0 && !airFlowing;
    }

    public void Respawn()
    {
        rb.velocity = Vector2.zero;
        transform.position = checkPointPos;
        deathCounter++;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
