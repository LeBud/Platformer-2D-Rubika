using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{

    [SerializeField] float breakTimer;
    [SerializeField] float respawnTimer;

    [SerializeField] Color color1;
    [SerializeField] Color color2;

    bool isPlaying;

    SpriteRenderer spriteRenderer;
    BoxCollider2D[] BoxCollider2D;

    private void Start()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponents<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlaying)
        {
            StartCoroutine(PlatformBreak());
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isPlaying)
        {
            StartCoroutine(PlatformBreak());
        }
    }*/

    IEnumerator PlatformBreak()
    {
        isPlaying = true;
        yield return new WaitForSeconds(breakTimer);

        spriteRenderer.color= color2;
        for(int i = 0; i < BoxCollider2D.Length; i++)
        {
            BoxCollider2D[i].enabled = false;
        }

        yield return new WaitForSeconds(respawnTimer);

        spriteRenderer.color= color1;
        for (int i = 0; i < BoxCollider2D.Length; i++)
        {
            BoxCollider2D[i].enabled = true;
        }

        isPlaying = false;
    }

}
