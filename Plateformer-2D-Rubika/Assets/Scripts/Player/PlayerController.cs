using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    [Header("Animator")]
    [SerializeField] Animator animator;

    bool jumpCut;
    [HideInInspector]
    public bool isJumping;
    [HideInInspector]
    public bool gliding;
    [HideInInspector]
    public bool glideJump;
    bool glideSpeed;
    bool airFlowing;
    bool onSlowPlatform;
    [HideInInspector]
    public bool jumpPadOn;
    bool jumpPadDoubleJump;
    [HideInInspector]
    public bool falling;

    float lastPressedJump;
    [HideInInspector]
    public float onGround;
    float airFlowForce;

    [HideInInspector]
    public float glideTime;
    [HideInInspector]
    public float flyTime;

    Vector2 moveInput;


    //AirFlow
    [HideInInspector]
    public bool inAirFlow;
    [HideInInspector]
    public Vector2 airFlowDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (PauseMenu.gameIsPause) return;

        if (PlayerDeath.respawning) return;

        //Timers
        onGround -= Time.deltaTime;
        lastPressedJump -= Time.deltaTime;
        if (gliding && !inAirFlow && rb.velocity.y < 0) glideTime -= Time.deltaTime;

        //ClampVelocity
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        MyInputs();
        CheckMethods();
        AnimationController();

        if (gliding && !inAirFlow && rb.velocity.y < 0) Glide();
    }

    void MyInputs()
    {
        //Movements Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput.x > .8f && (moveInput.y > .1f || moveInput.y < -.1f))
            moveInput.x = 1;
        else if (moveInput.x < -.8f && (moveInput.y > .1f || moveInput.y < -.1f))
            moveInput.x = -1;

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
        if (rb.velocity.y < 0 && onGround < 0 && !isJumping) falling = true;
        else falling = false;

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

        //Jumppad Check for double jump
        if (jumpPadOn) jumpPadDoubleJump = true;
        if (jumpPadDoubleJump && rb.velocity.y <= 12) jumpPadDoubleJump = false;

        #region MoveCam

        /*if (Idling()) centerCamTimer -= Time.deltaTime;
e   lse centerCamTimer = playerControllerData.timeToRecenter;

        if(rb.velocity.x > .1f)
        {
            vcBody.m_TrackedObjectOffset.x = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.x, playerControllerData.camOffsetX, playerControllerData.offsetSpeed * Time.deltaTime);
        }
        else if (rb.velocity.x < -.1f)
        {
            vcBody.m_TrackedObjectOffset.x = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.x, -playerControllerData.camOffsetX, playerControllerData.offsetSpeed * Time.deltaTime);
        }
        else if (Idling() && centerCamTimer < 0)
            vcBody.m_TrackedObjectOffset.x = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.x, 0, playerControllerData.offsetSpeed * Time.deltaTime);*/

        #endregion
    }

    void AnimationController()
    {
        if (gliding) animator.SetBool("Gliding", true);
        else if (falling)
        {
            animator.SetBool("Gliding", false);
            animator.SetBool("Fall", true);
        } 
        else animator.SetBool("Fall", false);

        if (!Idling()) animator.SetBool("Walk", true);
        else if (Idling()) animator.SetBool("Walk", false);
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

        animator.SetTrigger("Jump");
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
        if(rb.velocity.y > 0)
            upForce -= rb.velocity.y;

        rb.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
    }
    
    private void FixedUpdate()
    {
        if (PlayerDeath.respawning) return;

        //Movement
        if (airFlowing)
            AirFlowMovement();
        else
            Movement();

        //Set gravity
        if (airFlowing)
            SetGravityScale(playerControllerData.airFlowGravity);
        else if (falling)
            SetGravityScale(playerControllerData.fallGravity);
        else
            SetGravityScale(playerControllerData.normalGravitysScale);
    }

    void SetGravityScale(float gravity)
    {
        rb.gravityScale = gravity;
    }

    void Movement()
    {
        if (jumpPadOn) return;

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

    #region Boolean
    bool CanJumpGlide()
    {
        return (!isJumping || rb.velocity.y < 12) && glideJump && onGround < 0 && !jumpPadOn && !jumpPadDoubleJump;
    }

    bool CanJump()
    {
        return lastPressedJump > 0 && onGround > 0 && !isJumping && !airFlowing;
    }

    bool CanGlide()
    {
        return !isJumping && onGround < 0 && glideTime > 0 && !glideJump && rb.velocity.y <= 0;
    }

    bool Idling()
    {
        return rb.velocity.x < .1f && rb.velocity.x > -.1f;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
