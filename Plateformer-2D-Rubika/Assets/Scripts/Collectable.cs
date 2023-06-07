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

    float timeToTake = 3;
    float currentTime;

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
            //currentTime = Time.time;
            gameManager.collectableCount++;

            int listNum = ID - 1;

            gameManager.inGameCollectibles[listNum].taken = true;
            taken = true;
            FindObjectOfType<SaveSystem>().SaveCollectable();

            source.PlayOneShot(sound);
            StartCoroutine(UI.ShowCollectable());

            gameManager.RefreshList();
            gameObject.SetActive(false);
        }
    }

    /*private void LateUpdate()
    {
        if(taken)
        {
            transform.position = Vector3.Lerp(transform.position, UI.transform.position, 2 * Time.deltaTime);
            if (currentTime + timeToTake > Time.time)
            {
                gameManager.RefreshList();
                gameObject.SetActive(false);
            }
        }
    }*/

}
