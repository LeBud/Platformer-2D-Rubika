using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float speed;
    [SerializeField] float accel;
    [SerializeField] float deccel;

    Vector2 moveInput;

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
        moveInput.x = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        var movement = moveInput.x * speed;
        movement = movement - rb.velocity.x;

        float acceleration = Mathf.Abs(movement) > .01f ? accel : deccel;

        var force = new Vector2(movement * acceleration, rb.velocity.y);

        rb.AddForce(force, ForceMode2D.Force);
    }
}
