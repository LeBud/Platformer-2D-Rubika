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

    Vector2 inputMov;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MyInputs();
    }

    void MyInputs()
    {
        inputMov.x = Input.GetAxisRaw("Horizontal");
        inputMov.y = Input.GetAxisRaw("Vertical");

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
}
