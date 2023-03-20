using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRoom : MonoBehaviour
{

    [SerializeField] int roomNum;
    LevelManager roomManager;

    private void Awake()
    {
        roomManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        roomManager.currentRoom = roomNum;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        roomManager.currentRoom = roomNum;
    }

}
