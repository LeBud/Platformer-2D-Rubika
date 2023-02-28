using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFlow : MonoBehaviour
{

    [Header("Air Flow Settings")]
    [SerializeField] float airFlowForce;

    PlayerController playerController;

    Quaternion angle;
    Vector2 dir;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        angle = Quaternion.AngleAxis(transform.rotation.z, Vector2.right);
        dir = angle * transform.right;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.gliding)
        {
            playerController.inAirFlow = true;
            playerController.glideTime = playerController.playerControllerData.maxGlideTime;

            playerController.airFlowDir = dir;
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
