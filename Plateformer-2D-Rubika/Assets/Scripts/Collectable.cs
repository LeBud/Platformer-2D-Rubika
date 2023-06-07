using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    GameManager gameManager;

    [HideInInspector]
    public bool taken;
    public int ID;
    AudioSource source;
    public AudioClip sound;
    PlayerUI UI;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        source = GetComponent<AudioSource>();
        UI = FindObjectOfType<PlayerUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !taken)
        {
            gameManager.collectableCount++;

            int listNum = ID - 1;

            gameManager.inGameCollectibles[listNum].taken = true;
            gameManager.RefreshList();

            FindObjectOfType<SaveSystem>().SaveCollectable();

            source.PlayOneShot(sound);
            StartCoroutine(UI.ShowCollectable());

            gameObject.SetActive(false);
        }
    }

}
