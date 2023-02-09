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

    [Header("Hover")]
    [SerializeField] bool holdBtt;
    [SerializeField] float normalGravitysScale = 5;
    [SerializeField] float hoverGravityScale;
    [SerializeField] float hoverDownForce;
    [SerializeField] float maxHoverTime;
    [SerializeField] float hoverSpeedMult;
    [SerializeField] float curveMult;
    [SerializeField] AnimationCurve hoverCurve;

    [Header("Fly Ability")]
    [SerializeField] float flyGravity = 0;
    [SerializeField] float flyMaxTime;
    [SerializeField] float flySpeedMult;

    bool jumpCut;
    bool isJumping;
    bool hovering;
    bool isFlying;

    float lastPressedJump;
    float onGround;
    float hoverTime;
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


        //Hover Inputs
        if (holdBtt)
        {
            if (Input.GetButton("Jump") && !isJumping && onGround < 0 && hoverTime > 0)
                hovering = true;
            else if ((Input.GetButtonUp("Jump") && hovering) || hoverTime <= 0)
                hovering = false;
        }
        else
        {

            if (Input.GetButtonDown("Jump") && !isJumping && onGround < 0 && hoverTime > 0)
                hovering = true;
            else if ((Input.GetButtonUp("Jump") && hovering) || hoverTime <= 0)
                hovering = false;
        }
    }

    void CheckMethods()
    {
        //Jump Fields
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckLayerMask))
        {
            onGround = coyoteTime;
            jumpCut = false;
            hoverTime = maxHoverTime;
        }

        if (lastPressedJump > 0 && onGround > 0 && !isFlying)
            Jump();

        if (rb.velocity.y < 0)
            isJumping = false;

        if (jumpCut)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpCutForce);
            jumpCut = false;
        }


        //Hover Fields
        if (hovering)
            hoverTime -= Time.deltaTime;


        //Fly
        if (isFlying)
            flyTime -= Time.deltaTime;
        else
            flyTime = flyMaxTime;
        

        if (flyTime < 0)
            isFlying = false;
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

    private void FixedUpdate()
    {
        if(isFlying && flyTime > 0)
            Fly();
        else
            Movement();

        if (hovering)
            Hover(hoverGravityScale);
        else if (isFlying)
            Hover(flyGravity);
        else
            Hover(normalGravitysScale);
    }

    void Hover(float gravity)
    {
        rb.gravityScale = gravity;

        if (hovering)
            StartCoroutine(HoverCurve());
        
    }

    IEnumerator HoverCurve()
    {
        while (hoverTime <= maxHoverTime)
        {
            if (!hovering)
                yield break;

            float strenght = hoverCurve.Evaluate(hoverTime / maxHoverTime);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -strenght * curveMult, maxHoverTime));
            yield return null;
        }
    }

    void Movement()
    {
        float speedForce;

        if (hovering) speedForce = speed * hoverSpeedMult;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
