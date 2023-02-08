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

    bool jumpCut;
    bool isJumping;
    float lastPressedJump;
    float onGround;
    Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        onGround -= Time.deltaTime;
        lastPressedJump -= Time.deltaTime;

        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckLayerMask))
            onGround = coyoteTime; jumpCut = false;

        if (rb.velocity.y < 0) isJumping = false;

        MyInputs();

        if (lastPressedJump > 0 && onGround > 0) 
            Jump();

        if (jumpCut) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / jumpCutForce) ;
    }

    void MyInputs()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) 
            lastPressedJump = jumpBuffer;

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) jumpCut = true;
    }

    void Jump()
    {
        onGround = 0;
        lastPressedJump = 0;
        jumpCut = false;
        isJumping = true;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
