using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{

    [SerializeField] float breakTimer;
    [SerializeField] float respawnTimer;

    bool isPlaying;

    SpriteRenderer spriteRenderer;
    BoxCollider2D BoxCollider2D;

    private void Start()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        BoxCollider2D= GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlaying)
        {
            StartCoroutine(PlatformBreak());
        }
    }

    IEnumerator PlatformBreak()
    {
        isPlaying = true;
        yield return new WaitForSeconds(breakTimer);

        spriteRenderer.enabled = false;
        BoxCollider2D.enabled = false;

        yield return new WaitForSeconds(respawnTimer);

        spriteRenderer.enabled = true;
        BoxCollider2D.enabled = true;

        isPlaying = false;
    }

}
