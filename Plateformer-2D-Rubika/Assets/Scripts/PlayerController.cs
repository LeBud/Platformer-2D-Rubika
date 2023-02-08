using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float normalGravitysScale = 5;
    [SerializeField] float hoverGravityScale;
    [SerializeField] float maxHoverTime;

    bool jumpCut;
    bool isJumping;
    bool hovering;

    float lastPressedJump;
    float onGround;
    float hoverTime;

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
        //Movements and Jumps Inputs
        moveInput.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) 
            lastPressedJump = jumpBuffer;

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) jumpCut = true;

        //Hover Inputs
        if (Input.GetButton("Jump") && !isJumping && onGround < 0 && hoverTime > 0)
            hovering = true;
        else if ((Input.GetButtonUp("Jump") && hovering) || hoverTime <= 0)
            hovering = false;
        else hovering = false;
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

        if (lastPressedJump > 0 && onGround > 0)
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
        {
            Hover(hoverGravityScale);
            hoverTime -= Time.deltaTime;
        }
        else
            Hover(normalGravitysScale);
    }

    void Jump()
    {
        onGround = 0;
        lastPressedJump = 0;
        jumpCut = false;
        isJumping = true;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void Hover(float gravity)
    {
        rb.gravityScale = gravity;
     
        /*if (hovering)
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -1, 1));*/
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        var movement = moveInput.x * speed;

        float acceleration = Mathf.Abs(movement) > .01f ? accel : deccel;
        
        movement = movement - rb.velocity.x;

        var force = new Vector2(movement * acceleration, rb.velocity.y);

        rb.AddForce(force, ForceMode2D.Force);
    }
}
