using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    bool shrink;

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

            transform.parent = null;

            source.PlayOneShot(sound);
            StartCoroutine(UI.ShowCollectable());
            StartCoroutine(TakeCollectable());
        }
    }

    IEnumerator TakeCollectable()
    {
        float elapsedTime = 0;
        while (elapsedTime < timeToTake)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, UI.transform.position, 4 * Time.deltaTime);

            if (elapsedTime > 1.9f && !shrink)
                StartCoroutine(Shrink());

            yield return null;
            
        }

    }

    IEnumerator Shrink()
    {
        shrink = true;
        Light2D light = GetComponent<Light2D>();
        for (int i = 0; i < 10; i++)
        {
            transform.localScale += Vector3.one * 0.05f;
            yield return new WaitForSeconds(.05f);
        }

        for (int i = 0; i < 24; i++)
        {
            transform.localScale -= Vector3.one * 0.05f;
            light.intensity -= .025f;
            yield return new WaitForSeconds(.025f);
        }

        GetComponent<Collider2D>().enabled = false;
        gameManager.RefreshList();
        
        gameObject.SetActive(false);

    }

}
