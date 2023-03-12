using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [SerializeField] Vector2 bounceForce;

    PlayerController controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            controller = rb.GetComponent<PlayerController>();

            StartCoroutine(OnJumpPad());

            /*float xForce = rb.velocity.x;
            float yForce = rb.velocity.y;

            if(Mathf.Abs(xForce) < 5) xForce = 0;
            if(Mathf.Abs(yForce) < 5) yForce = 0;

            yForce = Mathf.Abs(yForce);

            if(bounceForce.x != 0)
            {
                if(Mathf.Sign(xForce) < 0 && bounceForce.x > 0) xForce = 0;
                else if(Mathf.Sign(xForce) > 0 && bounceForce.x < 0) xForce = 0;
            }*/

            //Vector2 additionalForce = new Vector2(xForce, yForce);

            rb.AddForce(bounceForce,ForceMode2D.Impulse);
        }
    }

    IEnumerator OnJumpPad()
    {
        controller.jumpPadOn = true;
        yield return new WaitForSeconds(.5f);
        controller.jumpPadOn = false;

    }

}
