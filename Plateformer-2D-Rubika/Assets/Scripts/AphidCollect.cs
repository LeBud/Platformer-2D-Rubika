using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidCollect : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player.flyTime >= player.playerControllerData.flyMaxTime)
                return;

            player.flyTime += player.playerControllerData.flyMaxTime / 3;

            StartCoroutine(Respawn());
        }
    }
    
    IEnumerator Respawn()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(4);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;

    }

}
