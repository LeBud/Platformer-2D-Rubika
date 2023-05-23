using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private void Awake()
    {
        player = FindObjectOfType<PlayerDeath>();
        lightSpot = GetComponentInChildren<Light2D>();
        sprite.sprite = unCheck;
        lightSpot.enabled = false;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        roomNumber = transform.parent.GetComponentInParent<LevelRoom>().roomNum;
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

        if (lightLevel)
        {
            player.lightCheckPoint = true;
            lightSpot.enabled = true;
        }
        else
            player.lightCheckPoint = false;
    }

}
