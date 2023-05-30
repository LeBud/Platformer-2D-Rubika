using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakPlatform : MonoBehaviour
{
    AchievementsCheck achievements;

    [SerializeField] float breakTimer;
    [SerializeField] float respawnTimer;

    [SerializeField] Color color1;
    [SerializeField] Color color2;

    bool isPlaying;

    SpriteRenderer spriteRenderer;
    BoxCollider2D[] BoxCollider2D;
    Animator animator;


    public string breakplat;
    public string respawnplat;

    AudioSource source;
    [SerializeField] AudioClip sound;
    private void Start()
    {
        achievements = FindObjectOfType<AchievementsCheck>();
        spriteRenderer= GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponents<BoxCollider2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

        //spriteRenderer.color= color2;
        //animator.Play("PlatformBreak");
        animator.Play(breakplat);
        source.PlayOneShot(sound);

        for(int i = 0; i < BoxCollider2D.Length; i++)
        {
            BoxCollider2D[i].enabled = false;
        }

        achievements.breakPlatformUse = true;

        yield return new WaitForSeconds(respawnTimer);

        animator.Play(respawnplat);

        ReactivePlatform();
        
        isPlaying = false;

        for (int i = 0; i < BoxCollider2D.Length; i++)
        {
            if (!BoxCollider2D[i].enabled)
            {
                ReactivePlatform();
            }
        }

    }

    void ReactivePlatform()
    {
        //spriteRenderer.color = color1;
        for (int i = 0; i < BoxCollider2D.Length; i++)
        {
            BoxCollider2D[i].enabled = true;
        }

    }

}
