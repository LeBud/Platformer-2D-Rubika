using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    Rigidbody2D rb;
    Vector3 spriteTransform;

    void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        spriteTransform = transform.localPosition;
    }

    private void LateUpdate()
    {
        if(rb.velocity.x > 0.5f)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = spriteTransform;
        }
        else if(rb.velocity.x < -0.5f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.localPosition = new Vector3(-spriteTransform.x, spriteTransform.y, spriteTransform.z);
        }
    }

}
