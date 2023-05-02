using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MantisCatch : MonoBehaviour
{

    [SerializeField] float rate;

    float nextTimeTo;
    bool isActive;

    PlayerDeath death;
    SpriteRenderer sprite;

    private void Start()
    {
        death = FindObjectOfType<PlayerDeath>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
        if(Time.time >= nextTimeTo)
        {
            nextTimeTo = Time.time + rate;
            Catch();
        }

    }

    void Catch()
    {
        if(isActive)
        {
            sprite.enabled = false;
            isActive = false;
        }
        else
        {
            sprite.enabled = true;
            isActive = true;
        }
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
