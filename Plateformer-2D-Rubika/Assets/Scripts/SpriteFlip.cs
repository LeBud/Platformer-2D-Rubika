using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        if(rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = new Vector3(.1f, -.11f, 0);
        }
        else if(rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.localPosition = new Vector3(-.1f, -.11f, 0);
        }
    }

}
