using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;

    [SerializeField] float fallGravityScale;

    private void Awake()
    {
        rb.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb.gravityScale = fallGravityScale;
        }
    }

}
