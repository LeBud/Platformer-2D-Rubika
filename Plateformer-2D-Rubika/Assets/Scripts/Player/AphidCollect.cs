using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidCollect : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LadyBugLight lady = collision.GetComponent<LadyBugLight>();
            PlayerController controller = collision.GetComponent<PlayerController>();

            if (lady.aphidCount >= controller.playerControllerData.maxAphid)
                return;

            lady.aphidCount++;

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
