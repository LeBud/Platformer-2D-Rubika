using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering.Universal;

public class CheckPoint : MonoBehaviour
{
    [Header("Checkpoints and Room number")]
    public int checkPointNum;
    public int roomNumber;

    [Header("Light Level")]
    public bool lightLevel;

    [SerializeField] SpriteRenderer sprite;
    PlayerDeath player;

    [SerializeField] Sprite unCheck;
    [SerializeField] Sprite check;

    Light2D lightSpot;

    Animator animator;
    
    [Header("particules")]
    private ParticleSystem particles;

    private ParticleSystem.EmissionModule emission;

    AudioSource source;
    public AudioClip sound;

    private void Awake()
    {
        player = FindObjectOfType<PlayerDeath>();
        lightSpot = GetComponentInChildren<Light2D>();
        sprite.sprite = unCheck;
        //lightSpot.enabled = false;
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
        emission = GetComponentInChildren<ParticleSystem>().emission;
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        roomNumber = transform.parent.GetComponentInParent<LevelRoom>().roomNum;
        emission.enabled = false;
    }

    private void OnEnable()
    {
        if(checkPointNum <= player.currentCheckPoint)
        {
            Active();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            if(checkPointNum > player.currentCheckPoint)
            {
                source.PlayOneShot(sound);
                Active();
                FindObjectOfType<SaveSystem>().SaveData();
            }
        }
    }

    public void Active()
    {
        //sprite.sprite = check;
        player.checkPointPos = transform.position;
        player.currentCheckPoint = checkPointNum;
        player.checkPointRoom = roomNumber;


        animator.Play("Checkpoint");
        emission.enabled = true;
        //particles.Emit(30);
        

        if (lightLevel)
        {
            player.lightCheckPoint = true;
            lightSpot.enabled = true;
        }
        /*else
            player.lightCheckPoint = false;*/

    }

}
