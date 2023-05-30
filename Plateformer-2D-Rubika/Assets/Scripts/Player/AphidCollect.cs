using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidCollect : MonoBehaviour
{

    [SerializeField] float respawnTime;
    public bool canRespawn;
    AudioSource source;
    public AudioClip sound;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LadyBugLight lady = collision.GetComponent<LadyBugLight>();
            PlayerController controller = collision.GetComponent<PlayerController>();

            if (lady.oldSystem)
            {
                if (lady.aphidCount >= controller.playerControllerData.maxAphid)
                    return;

                lady.aphidCount++;
            }
            else
            {
                if (lady.aphidCharge >= controller.playerControllerData.maxAphidCharge)
                    return;
                source.PlayOneShot(sound);
                lady.aphidCharge += 100;
            }

            if(canRespawn)
                StartCoroutine(Respawn());
            else
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                GetComponent<CircleCollider2D>().enabled = false;
            }

        }
    }
    
    IEnumerator Respawn()
    {

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<CircleCollider2D>().enabled = false;

        yield return new WaitForSeconds(respawnTime);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<CircleCollider2D>().enabled = true;

    }

    public void RespawnAphidWhenDead()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<CircleCollider2D>().enabled = true;
    }

}
