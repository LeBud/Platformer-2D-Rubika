using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantysEnablerDisabler : MonoBehaviour
{
    public bool Enabler;
    public static bool mantisEnable;
    public int waypointStart;
    public int jumpPointReset;


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
                vCam.SetActive(true);

                MantysFollower script = mantis.GetComponent<MantysFollower>();

                script.respawnCheckPoint = waypointStart;
                script.currentWaypoint = waypointStart;
                script.jumpPointNum = jumpPointReset;
                mantis.transform.position = script.respawnPos[waypointStart].position;
            }
            else
            {
                //mantisEnable = false;
                vCam.SetActive(false);
            }
        }
    }

}
