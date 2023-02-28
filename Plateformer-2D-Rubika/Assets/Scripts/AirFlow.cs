using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFlow : MonoBehaviour
{

    [Header("Air Flow Settings")]
    [SerializeField] float airFlowForce;

    Rigidbody2D rb;
    PlayerController playerController;

    Quaternion angle;
    Vector2 dir;

    private void Awake()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = rb.transform.GetComponent<PlayerController>();

        angle = Quaternion.AngleAxis(transform.rotation.z, Vector2.right);
        dir = angle * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.gliding)
        {
            rb.velocity = new Vector2(rb.velocity.x / 4, rb.velocity.y / 4);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.gliding)
        {
            playerController.glideTime = playerController.playerControllerData.maxGlideTime;
            playerController.inAirFlow = true;

            rb.velocity = new Vector2(dir.x * airFlowForce, dir.y * airFlowForce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.gliding)
        {
            playerController.inAirFlow = false;
        }
    }

}
