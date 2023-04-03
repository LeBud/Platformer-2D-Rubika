using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    public int checkPointNum;
    public int roomNumber;
    SpriteRenderer sprite;
    PlayerDeath player;
    

    private void Awake()
    {
        player = FindObjectOfType<PlayerDeath>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            if(checkPointNum > player.currentCheckPoint)
            {
                sprite.color = new Vector4(1, 1, 0.1568628f, 1);
                player.checkPointPos = transform.position;
                player.currentCheckPoint = checkPointNum;
                player.checkPointRoom = roomNumber;
                FindObjectOfType<SaveSystem>().SaveData();
            }
        }
    }

}
