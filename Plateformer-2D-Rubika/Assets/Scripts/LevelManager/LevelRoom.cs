using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRoom : MonoBehaviour
{

    public int roomNum;
    LevelManager roomManager;

    private void Awake()
    {
        roomManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        roomManager.currentRoom = roomNum;
    }

}
