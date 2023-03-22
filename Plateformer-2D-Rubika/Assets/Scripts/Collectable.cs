using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public bool taken;
    public bool checkPointSave;

    private void OnEnable()
    {
        taken = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCollectable.collectable++;
            taken = true;
            FindObjectOfType<PlayerCollectable>().collectablesList.Add(this);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

}
