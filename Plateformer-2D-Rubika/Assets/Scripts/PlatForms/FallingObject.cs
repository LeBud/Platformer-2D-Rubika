using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    NewAchievementSystem achievements;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] float fallGravityScale = 5;
    [SerializeField] float delay = 0.1f;

    Vector2 startPos;
    bool resetting;
    bool falled;

    AudioSource source;
    public AudioClip sound;

    private void Awake()
    {
        achievements = FindObjectOfType<NewAchievementSystem>();
        rb.gravityScale = 0;
        startPos = rb.position;
        source= GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !resetting && !falled)
        {
            StartCoroutine(Fall());
        }
    }

    IEnumerator Fall()
    {
        falled = true;
        yield return new WaitForSeconds(delay);
        rb.gravityScale = fallGravityScale;

        source.PlayOneShot(sound);

        achievements.fallObjectUse = true;
    }

    public IEnumerator ResetFall()
    {
        resetting= true;

        rb.gravityScale = 0;
        rb.position = startPos;

        yield return new WaitForSeconds(1);

        falled = false;
        resetting= false;
    }

}
