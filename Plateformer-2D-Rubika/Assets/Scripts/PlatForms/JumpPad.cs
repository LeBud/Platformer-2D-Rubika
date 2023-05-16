using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [SerializeField] Vector2 bounceForce;

    PlayerController controller;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            controller = rb.GetComponent<PlayerController>();

            StartCoroutine(OnJumpPad());

            rb.velocity = Vector2.zero;

            rb.AddForce(bounceForce,ForceMode2D.Impulse);

            animator.Play("Bumper");
        }
    }

    IEnumerator OnJumpPad()
    {
        controller.glideJump = true;
        controller.glideTime = controller.playerControllerData.maxGlideTime;

        if(bounceForce.x != 0)
            controller.jumpPadOn = true;

        yield return new WaitForSeconds(.5f);

        controller.jumpPadOn = false;
    }

}
