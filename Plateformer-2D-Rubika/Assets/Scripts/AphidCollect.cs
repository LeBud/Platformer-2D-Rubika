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

            Destroy(gameObject);
        }
    }

}
