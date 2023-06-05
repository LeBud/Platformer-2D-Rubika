using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MantisCatch : MonoBehaviour
{

    [SerializeField] float activeTime;
    [SerializeField] float deactiveTime;

    float nextTimeTo;
    bool isActive;

    PlayerDeath death;
    SpriteRenderer sprite;
    Animator anim;

    AudioSource source;
    public AudioClip sound;
    private void Start()
    {
        death = FindObjectOfType<PlayerDeath>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim= GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        if(Time.time >= nextTimeTo)
        {
            nextTimeTo = Time.time + deactiveTime;
            StartCoroutine(Catch());
        }

    }

    IEnumerator Catch()
    {
        isActive = true;
        anim.Play("MantisPawOn");

        source.PlayOneShot(sound);

        yield return new WaitForSeconds(activeTime);
        
        anim.Play("MantisPawOff");
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(isActive)
            {
                StartCoroutine(death.Respawn());
            }
        }
    }

}
