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

    private void Awake()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = rb.transform.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.gliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.gliding)
        {
            playerController.inAirFlow = true;
            rb.velocity = new Vector2(rb.velocity.x, airFlowForce);
            //rb.AddForce(transform.right * airFlowForce, ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.gliding)
        {
            playerController.inAirFlow = false;
            //rb.AddForce(transform.right * airFlowForce, ForceMode2D.Impulse);
        }
    }

}
