using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    AchievementsCheck achievements;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] float fallGravityScale = 5;
    [SerializeField] float delay = 0.1f;

    Vector2 startPos;
    bool resetting;

    private void Awake()
    {
        rb.gravityScale = 0;
        startPos = rb.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !resetting)
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(delay);
        rb.gravityScale = fallGravityScale;

        achievements.fallObjectUse = true;
    }

    public IEnumerator ResetFall()
    {
        resetting= true;

        rb.gravityScale = 0;
        rb.position = startPos;

        yield return new WaitForSeconds(.01f);
        
        resetting= false;
    }

}
