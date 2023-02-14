using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float accel;
    [SerializeField] float deccel;

    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float coyoteTime = .1f;
    [SerializeField] float jumpBuffer = .1f;
    [SerializeField] float jumpCutForce = .1f;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundCheckLayerMask;

    [Header("Glide")]
    [SerializeField] bool holdBtt;
    [SerializeField] bool canGlideJump;
    [SerializeField] float glideJumpForce;
    [SerializeField] float maxGlideTime;
    [SerializeField] float glideSpeedMult;
    [SerializeField] float curveMult;
    [SerializeField] AnimationCurve glideCurve;

    [Header("Fly Ability")]
    [SerializeField] float flyMaxTime;
    [SerializeField] float flySpeedMult;

    [Header("Gravity")]
    [SerializeField] float normalGravitysScale = 5;
    [SerializeField] float glideGravityScale;
    [SerializeField] float flyGravity = 0;


    bool jumpCut;
    bool isJumping;
    bool gliding;
    bool isFlying;
    bool glideJump;

    float lastPressedJump;
    float onGround;
    float glideTime;
    float flyTime;

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


        MyInputs();
        CheckMethods();

        if (gliding)
            Glide();
    }

    void MyInputs()
    {
        //Movements Input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");


        //Fly Input
        if(Input.GetButtonDown("Fire1"))
            isFlying = true;


        //Jumps Inputs
        if (Input.GetButtonDown("Jump")) 
            lastPressedJump = jumpBuffer;

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) jumpCut = true;


        //Glide Inputs

        if (canGlideJump)
        {
            if (holdBtt)
            {
                if (Input.GetButton("Jump") && !isJumping && glideJump && onGround < 0)
                    GlideJump();
            }
            else
            {
                if (Input.GetButtonDown("Jump") && !isJumping && glideJump && onGround < 0)
                    GlideJump();
            }
        }

        if (holdBtt)
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
    }

    void CheckMethods()
    {
        //Jump Fields
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckLayerMask))
        {
            onGround = coyoteTime;
            jumpCut = false;
            glideTime = maxGlideTime;
        }

        if (lastPressedJump > 0 && onGround > 0 && !isFlying) Jump();

        if (rb.velocity.y < 0) isJumping = false;

        if (jumpCut)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpCutForce);
            jumpCut = false;
        }


        //Glide Fields
        if (gliding) glideTime -= Time.deltaTime;

        if (canGlideJump)
        {
            if (onGround > 0) glideJump = true;
        }
        else
            glideJump = false;

        //Fly
        if (isFlying) flyTime -= Time.deltaTime;
        else flyTime = flyMaxTime;
        

        if (flyTime < 0) isFlying = false;
    }

    void Jump()
    {
        onGround = 0;
        lastPressedJump = 0;
        jumpCut = false;
        isJumping = true;

        float force = jumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    void Glide()
    {
        while (glideTime <= maxGlideTime)
        {
            if (!gliding)
                break;

            float strenght = glideCurve.Evaluate(glideTime / maxGlideTime);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -strenght * curveMult, maxGlideTime));
            return;
        }
    }

    void GlideJump()
    {
        glideJump = false;

        float upForce = glideJumpForce;
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
            SetGravityScale(glideGravityScale);
        else if (isFlying)
            SetGravityScale(flyGravity);
        else
            SetGravityScale(normalGravitysScale);
    }

    void SetGravityScale(float gravity)
    {
        rb.gravityScale = gravity;
    }

    void Movement()
    {
        float speedForce;

        if (gliding) speedForce = speed * glideSpeedMult;
        else speedForce = speed;

        var movement = moveInput.x * speedForce;

        float acceleration = Mathf.Abs(movement) > .01f ? accel : deccel;
        
        movement = movement - rb.velocity.x;

        var force = new Vector2(movement * acceleration, rb.velocity.y);

        rb.AddForce(force, ForceMode2D.Force);
    }

    void Fly()
    {
        float speedForce = speed * flySpeedMult;

        var horMov = moveInput.x * speedForce;
        var verMov = moveInput.y * speedForce;

        float acceleration = Mathf.Abs(horMov) > .01f || Mathf.Abs(verMov) > .01f ? accel : deccel;

        horMov = horMov - rb.velocity.x;
        verMov = verMov - rb.velocity.y;

        var force = new Vector2(horMov * acceleration, verMov * acceleration);

        rb.AddForce(force, ForceMode2D.Force);
    }

    bool CanGlide()
    {
        return !isJumping && onGround < 0 && glideTime > 0 && !isFlying && !glideJump && rb.velocity.y <= 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
