using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MantisAI : MonoBehaviour
{
    [Header("PathFinding")]
    [SerializeField] Transform target;
    [SerializeField] float activateDistance;
    [SerializeField] float pathUpdateSeconds;

    [Header("Physics")]
    [SerializeField] float speed;
    [SerializeField] float nextWaypointDistance;
    [SerializeField] float jumpNodeHeightRequirement;
    [SerializeField] float jumpModifier;
    [SerializeField] float jumpCheckOffset;

    [Header("Custom Behavior")]
    [SerializeField] bool followEnable = true;
    [SerializeField] bool jumpEnable = true;
    [SerializeField] bool directionLook = true;

    Path path;
    int currentWaypoint;
    bool isGrounded;
    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        seeker= GetComponent<Seeker>();
        rb= GetComponent<Rigidbody2D>();


        InvokeRepeating("UpdatePath", 0, pathUpdateSeconds);
    }

    void UpdatePath()
    {
        if(followEnable && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset);

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = dir * speed * Time.deltaTime;

        if (jumpEnable && isGrounded)
            if (dir.y > jumpNodeHeightRequirement)
                rb.AddForce(Vector2.up * speed * jumpModifier);

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;


        //Add changement de direction du sprite + cam
    }

    private void FixedUpdate()
    {
        if(TargetInDistance() && followEnable)
        {
            PathFollow();
        }
    }


    bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.position) < activateDistance;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}
