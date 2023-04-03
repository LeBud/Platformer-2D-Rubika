using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    [SerializeField] PlayerController controller;
    Rigidbody2D rb;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = controller.GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        if (controller.isJumping)
            animator.SetTrigger("Jump");
        else if (controller.falling)
            animator.SetTrigger("Fall");
        /*else if (controller.onGround > 0)
            animator.SetTrigger("Land");*/
        else if (rb.velocity.x < -.1f || rb.velocity.x > .1f)
            animator.SetBool("Walk", true);
        else
            animator.SetBool("Walk", false);
    }

}
