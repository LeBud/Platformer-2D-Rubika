using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
            float yForce = rb.velocity.y;

            if(Mathf.Abs(xForce) < 5) xForce = 0;
            if(Mathf.Abs(yForce) < 5) yForce = 0;

            yForce = Mathf.Abs(yForce);
            
            rb.AddForce(new Vector2(xForce + bounceForce.x, yForce + bounceForce.y),ForceMode2D.Impulse);
        }
    }

}
