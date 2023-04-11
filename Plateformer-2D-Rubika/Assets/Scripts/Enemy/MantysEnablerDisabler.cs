using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantysEnablerDisabler : MonoBehaviour
{
    public bool Enabler;
    public static bool mantisEnable;

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
            }
            else
            {
                mantisEnable = false;
                mantis.SetActive(false);
                vCam.SetActive(false);
            }
        }
    }

}
