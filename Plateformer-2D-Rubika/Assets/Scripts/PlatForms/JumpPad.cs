using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    [SerializeField] Vector2 bounceForce;

    PlayerController controller;
    Animator animator;
    AudioSource source;
    public AudioClip sound;
    private void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
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

        }
    }

    IEnumerator OnJumpPad()
    {
        FindObjectOfType<NewAchievementSystem>().jumpPadUse = true;

        controller.glideJump = true;
        controller.glideTime = controller.playerControllerData.maxGlideTime;

        //animator.Play("Bumper");
        animator.SetTrigger("Bump");
        source.PlayOneShot(sound);

        if (bounceForce.x != 0)
            controller.jumpPadOn = true;
        controller.jumpPadVer = true;

        yield return new WaitForSeconds(.5f);

        controller.jumpPadOn = false;
        controller.jumpPadVer = false;
    }

}
