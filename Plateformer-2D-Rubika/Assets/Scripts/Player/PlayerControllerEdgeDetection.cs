using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerControllerEdgeDetection : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerController controller;

    [Header("Edge Check")]
    [SerializeField] float range;
    [SerializeField] Vector3 edgeDetectionSize;
    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;
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
            if(Physics2D.Raycast(leftEdge.position, Vector2.up, range, edgeCheckLayerMask) && !LeftInnerCheck() && rb.velocity.x < 5)
                MovePlayer(rb.velocity, edgeDetectionSize.x);
            else if (Physics2D.Raycast(rightEdge.position, Vector2.up, range, edgeCheckLayerMask) && !RightInnerCheck() && rb.velocity.x > -5)
                MovePlayer(rb.velocity, -edgeDetectionSize.x);

        }


        DrawRaycast();
    }

    void DrawRaycast()
    {
        Debug.DrawRay(leftEdge.position, Vector2.up * range, Color.green);
        Debug.DrawRay(leftEdge.position + edgeDetectionSize, Vector2.up * range, Color.yellow);

        Debug.DrawRay(rightEdge.position, Vector2.up * range, Color.green);
        Debug.DrawRay(rightEdge.position - edgeDetectionSize, Vector2.up * range, Color.yellow);
    }

    void MovePlayer(Vector2 velocity, float pos)
    {
        rb.velocity = Vector2.zero;

        Vector2 currentPos = transform.position;
        currentPos.x += pos;
        transform.position = currentPos;

        velocity.x = 0;

        rb.velocity = velocity;
    }

    bool LeftInnerCheck()
    {
        return Physics2D.Raycast(leftEdge.position + edgeDetectionSize, Vector2.up, range, edgeCheckLayerMask);
    }

    bool RightInnerCheck()
    {
        return Physics2D.Raycast(leftEdge.position - edgeDetectionSize, Vector2.up, range, edgeCheckLayerMask);
    }

}
