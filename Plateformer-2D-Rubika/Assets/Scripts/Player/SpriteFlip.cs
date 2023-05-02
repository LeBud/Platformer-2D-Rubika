using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    Rigidbody2D rb;
    
    [HideInInspector]
    public bool facingRight;

    CameraFollowObject followObject;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        followObject = FindObjectOfType<CameraFollowObject>();
    }

    private void FixedUpdate()
    {
        TurnCheck();
    }

    void TurnCheck()
    {
        if (rb.velocity.x > 0.25f && !facingRight)
            Turn();
        else if (rb.velocity.x < -0.25f && facingRight)
            Turn();
    }

    void Turn()
    {
        if (facingRight)
        {
            Vector3 rotation = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
            facingRight = !facingRight;

            followObject.CallTurn();
        }
        else
        {
            Vector3 rotation = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotation);
            facingRight = !facingRight;

            followObject.CallTurn();
        }
    }

}
