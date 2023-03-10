using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerControllerEdgeDetection : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerController controller;

    [Header("Edge Check")]
    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;
    [SerializeField] Vector2 edgeCheckSize;
    [SerializeField] Transform topEdge;
    [SerializeField] Vector2 topCheckSize;
    [SerializeField] LayerMask edgeCheckLayerMask;


    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        rb= GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (controller.isJumping)
        {
            if (Physics2D.OverlapBox(leftEdge.position, edgeCheckSize, 0, edgeCheckLayerMask) && !TopPlayer())
            {
                MovePlayer(rb.velocity,edgeCheckSize);
            }
            else if (Physics2D.OverlapBox(rightEdge.position, edgeCheckSize, 0, edgeCheckLayerMask) && !TopPlayer())
            {
                MovePlayer(rb.velocity, -edgeCheckSize);
            }
        }
        
    }

    void MovePlayer(Vector2 velocity, Vector2 pos)
    {
        rb.velocity = velocity;

        Vector2 currentPos = transform.position;

        rb.velocity = velocity;

        currentPos += pos;

        rb.velocity = velocity;

        transform.position = currentPos;

        rb.velocity = velocity;
    }

    bool TopPlayer()
    {
        return Physics2D.OverlapBox(topEdge.position, topCheckSize, 0, edgeCheckLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(leftEdge.position, edgeCheckSize);
        Gizmos.DrawWireCube(rightEdge.position, edgeCheckSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(topEdge.position, topCheckSize);
    }

}
