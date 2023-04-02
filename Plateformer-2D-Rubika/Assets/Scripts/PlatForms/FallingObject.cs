using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;

    [SerializeField] float fallGravityScale = 5;
    [SerializeField] float delay = 0.1f;

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
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(delay);
        rb.gravityScale = fallGravityScale;
    }

    public void ResetFall()
    {
        rb.gravityScale = 0;
        rb.position = startPos;
    }

}
