using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;

    [SerializeField] float fallGravityScale;

    Vector2 startPos;

    private void Awake()
    {
        rb.gravityScale = 0;
        startPos = rb.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.gravityScale = fallGravityScale;
        }
    }

    public void ResetFall()
    {
        rb.gravityScale = 0;
        rb.position = startPos;
    }

}
