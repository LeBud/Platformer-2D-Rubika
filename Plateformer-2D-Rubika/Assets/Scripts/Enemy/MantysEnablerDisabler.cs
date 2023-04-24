using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantysEnablerDisabler : MonoBehaviour
{
    public bool Enabler;
    public static bool mantisEnable;
    public int waypointStart;


    [SerializeField] GameObject mantis;
    [SerializeField] GameObject vCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Enabler)
            {
                mantisEnable = true;
                mantis.SetActive(true);

                MantysFollower script = mantis.GetComponent<MantysFollower>();

                script.respawnCheckPoint = waypointStart;
                script.currentWaypoint = waypointStart;
                mantis.transform.position = script.mantysTransform[waypointStart].position;
                vCam.SetActive(true);
                Debug.Log(2);
            }
            else
            {
                mantisEnable = false;
                vCam.SetActive(false);
                Debug.Log(3);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Enabler)
            {
                mantisEnable = true;
                mantis.SetActive(true);

                MantysFollower script = mantis.GetComponent<MantysFollower>();

                script.respawnCheckPoint = waypointStart;
                script.currentWaypoint = waypointStart;
                mantis.transform.position = script.mantysTransform[waypointStart].position;
                vCam.SetActive(true);
                Debug.Log(2);
            }
            else
            {
                mantisEnable = false;
                vCam.SetActive(false);
                Debug.Log(3);
            }
        }

    }
}
