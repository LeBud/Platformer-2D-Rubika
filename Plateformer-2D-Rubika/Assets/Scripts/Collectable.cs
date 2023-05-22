using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    GameManager gameManager;

    [HideInInspector]
    public bool taken;
    [HideInInspector]
    public int ID;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !taken)
        {
            gameManager.collectableCount++;

            gameManager.inGameCollectibles[ID].taken = true;
            gameManager.RefreshList();

            FindObjectOfType<SaveSystem>().SaveCollectable();

            gameObject.SetActive(false);
        }
    }

}
