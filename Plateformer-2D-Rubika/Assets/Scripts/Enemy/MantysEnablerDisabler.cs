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
                //mantis.GetComponent<MantysFollower>().enabled = true;
                mantis.GetComponent<MantysFollower>().currentWaypoint = waypointStart;
                mantis.transform.position = mantis.GetComponent<MantysFollower>().mantysTransform[waypointStart].position;
                vCam.SetActive(true);
            }
            else
            {
                mantisEnable = false;
                //mantis.GetComponent<MantysFollower>().enabled= false;
                vCam.SetActive(false);
            }
        }
    }

}
