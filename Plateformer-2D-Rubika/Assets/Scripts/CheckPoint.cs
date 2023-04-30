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

    private void Awake()
    {
        player = FindObjectOfType<PlayerDeath>();
        lightSpot = GetComponentInChildren<Light2D>();
        roomNumber = transform.parent.GetComponentInParent<LevelRoom>().roomNum;
        sprite.sprite = unCheck;
        lightSpot.enabled = false;
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
        sprite.sprite = check;
        player.checkPointPos = transform.position;
        player.currentCheckPoint = checkPointNum;
        player.checkPointRoom = roomNumber;

        if (lightLevel)
        {
            player.lightCheckPoint = true;
            lightSpot.enabled = true;
        }
        else
            player.lightCheckPoint = false;
    }

}
