using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Movements Settings")]
    [SerializeField] float runSpeed;
    [SerializeField] float lerpAmount;
    [SerializeField] float accelRate;
    [SerializeField] float deccelRate;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce;
    [SerializeField] float coyoteTime;
    [SerializeField] float jumpInputBuffer;
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundCheckLayer;
    bool isJumping;

    float lastTimeOnGround;
    float lastJumpPressed;

    Vector2 inputMov;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lastTimeOnGround -= Time.deltaTime;
        lastJumpPressed -= Time.deltaTime;

        MyInputs();

        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckLayer)) lastTimeOnGround = coyoteTime;

        if (lastJumpPressed > 0 && lastTimeOnGround > 0) Jump();
    }

    void MyInputs()
    {
        inputMov.x = Input.GetAxisRaw("Horizontal");
        inputMov.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump")) lastJumpPressed = jumpInputBuffer;
    }

    void Jump()
    {
        lastTimeOnGround = 0;
        lastJumpPressed = 0;

        float force = jumpForce;
        if (rb.velocity.y < 1)
            force -= rb.velocity.y;

        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float targetSpeed = inputMov.x * runSpeed;
        targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount);

        float acceleration = (Mathf.Abs(targetSpeed) > 0.01f ) ? accelRate : deccelRate;

        float clampSpeed = targetSpeed - rb.velocity.x;

        float appliedForce = clampSpeed * acceleration;

        rb.AddForce(appliedForce * Vector2.right, ForceMode2D.Force);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }

}
