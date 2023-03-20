using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!PlayerDeath.respawning)
                StartCoroutine(collision.GetComponent<PlayerDeath>().Respawn());
        }
           
    }
}
