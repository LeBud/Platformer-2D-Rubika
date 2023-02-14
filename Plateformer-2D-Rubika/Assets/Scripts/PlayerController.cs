using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Controller Data")]
    public PlayerControllerData playerControllerData;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundCheckLayerMask;


    bool jumpCut;
    bool isJumping;
    bool gliding;
    bool isFlying;
    bool glideJump;

    float lastPressedJump;
    float onGround;

    [HideInInspector]
    public float glideTime;
    [HideInInspector]
    public float flyTime;

    Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Timers
        onGround -= Time.deltaTime;
        lastPressedJump -= Time.deltaTime;
        if (gliding) glideTime -= Time.deltaTime;
        if (isFlying) flyTime -= Time.deltaTime;

        MyInputs();
        CheckMethods();

        if (gliding) Glide();
    }

    void MyInputs()
    {
        //Movements Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");


        //Fly Input
        if (Input.GetButtonDown("Fire1") && CanFly())
        {
            flyTime = playerControllerData.flyTimePerAphid;
            playerControllerData.aphidAmount--;
        }


        //Jumps Inputs
        if (Input.GetButtonDown("Jump")) 
            lastPressedJump = playerControllerData.jumpBuffer;

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) jumpCut = true;


        #region GlideInputs
        //Glide Inputs
        if (playerControllerData.canGlideJump)
        {
            /*if (holdBtt)
            {
                if (Input.GetButton("Jump") && !isJumping && glideJump && onGround < 0)
                    GlideJump();
            }
            else
            {*/
                if (Input.GetButtonDown("Jump") && !isJumping && glideJump && onGround < 0)
                    GlideJump();
            //}
        }

        if (playerControllerData.holdBtt)
        {
            if (Input.GetButton("Jump") && CanGlide())
                gliding = true;
            else if ((Input.GetButtonUp("Jump") && gliding) || glideTime <= 0)
                gliding = false;
        }
        else
        {

            if (Input.GetButtonDown("Jump") && CanGlide())
                gliding = true;
            else if ((Input.GetButtonUp("Jump") && gliding) || glideTime <= 0)
                gliding = false;
        }
        #endregion
    }

    void CheckMethods()
    {
        //Jump Fields
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckLayerMask))
        {
            onGround = playerControllerData.coyoteTime;
            jumpCut = false;
            glideTime = playerControllerData.maxGlideTime;
            glideJump = true;
        }

        if (lastPressedJump > 0 && onGround > 0 && !isFlying) Jump();

        if (rb.velocity.y < 8) isJumping = false;

        if (jumpCut)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / playerControllerData.jumpCutForce);
            jumpCut = false;
        }


        //Glide Fields
        if (!playerControllerData.canGlideJump)
            glideJump = false;


        //Fly
        if (flyTime >= 0) isFlying = true;
        else isFlying = false;

        if (flyTime > playerControllerData.flyMaxTime) flyTime = playerControllerData.flyMaxTime;
    }

    void Jump()
    {
        onGround = 0;
        lastPressedJump = 0;
        jumpCut = false;
        isJumping = true;

        float force = playerControllerData.jumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void Glide()
    {
        while (glideTime <= playerControllerData.maxGlideTime)
        {
            if (!gliding)
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
        if(isFlying && flyTime > 0)
            Fly();
        else
            Movement();

        //Set gravity
        if (gliding)
            SetGravityScale(playerControllerData.glideGravityScale);
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

        if (gliding) speedForce = playerControllerData.speed * playerControllerData.glideSpeedMult;
        else speedForce = playerControllerData.speed;

        var movement = moveInput.x * speedForce;

        float acceleration = Mathf.Abs(movement) > .01f ? playerControllerData.accel : playerControllerData.deccel;
        
        movement = movement - rb.velocity.x;

        var force = new Vector2(movement * acceleration, rb.velocity.y);

        rb.AddForce(force, ForceMode2D.Force);
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

    bool CanGlide()
    {
        return !isJumping && onGround < 0 && glideTime > 0 && !isFlying && !glideJump && rb.velocity.y <= 0;
    }

    bool CanFly()
    {
        return playerControllerData.aphidAmount > 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
