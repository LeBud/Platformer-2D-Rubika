using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    public int checkPointNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if(checkPointNum > player.currentCheckPoint)
            {
                player.checkPointPos = transform.position;
                player.currentCheckPoint = checkPointNum;
            }
        }
    }

}
