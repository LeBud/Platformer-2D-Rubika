using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    GameManager gameManager;
    [HideInInspector]
    public bool taken;
    [HideInInspector]
    public int num;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.collectableNum++;
            taken = true;

            gameManager.collectables[num].taken = taken;

            gameObject.SetActive(false);
        }
    }

}
