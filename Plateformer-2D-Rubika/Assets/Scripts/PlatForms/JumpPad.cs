using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [SerializeField] Vector2 bounceForce;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            float xForce = rb.velocity.x;

            if(Mathf.Abs(xForce) < 5) xForce = 0;
            
            rb.AddForce(new Vector2(xForce + bounceForce.x, bounceForce.y),ForceMode2D.Impulse);
        }
    }

}
