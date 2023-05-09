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

    private void Start()
    {
        death = FindObjectOfType<PlayerDeath>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
        sprite.enabled = true;
        isActive = true;

        yield return new WaitForSeconds(activeTime);
        
        sprite.enabled = false;
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
