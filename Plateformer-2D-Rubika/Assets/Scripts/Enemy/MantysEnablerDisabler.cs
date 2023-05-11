using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantysEnablerDisabler : MonoBehaviour
{
    public bool Enabler;
    public int waypointStart;

    public static bool mantisEnable;

    [SerializeField] GameObject mantis;

    public bool used;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (used) return;

            if(Enabler)
            {
                mantisEnable = true;
                mantis.SetActive(true);

                MantisAI script = mantis.GetComponent<MantisAI>();

                script.respawnWP = waypointStart;
                script.currentWP = waypointStart;
                mantis.transform.position = script.waypoints[waypointStart].position;
                used = true;
            }
            else
            {
                mantisEnable = false;
                used = true;
            }
        }
    }

}
