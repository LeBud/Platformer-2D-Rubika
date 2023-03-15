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
        if (rb.velocity.y >= 0)
        {
            if (Physics2D.OverlapBox(leftEdge.position, edgeCheckSize, 0, edgeCheckLayerMask) && !TopPlayer())
            {
                MovePlayer(rb.velocity,edgeCheckSize.x);
            }
            else if (Physics2D.OverlapBox(rightEdge.position, edgeCheckSize, 0, edgeCheckLayerMask) && !TopPlayer())
            {
                MovePlayer(rb.velocity, -edgeCheckSize.x);
            }
        }
        
    }

    void MovePlayer(Vector2 velocity, float pos)
    {
        Vector2 currentPos = transform.position;
        currentPos.x += pos;
        transform.position = currentPos;

        rb.velocity = velocity;

        StartCoroutine(Velocity(velocity));
    }

    IEnumerator Velocity(Vector2 velocity)
    {
        yield return new WaitForSeconds(.01f);
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
