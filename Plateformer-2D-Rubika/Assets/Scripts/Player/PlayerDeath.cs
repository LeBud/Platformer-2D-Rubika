using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    public static bool respawning;

    Rigidbody2D rb;
    LevelManager levelManager;
    LadyBugLight ladyBugLight;

    [Header("Virtual Camera")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [Header("Fade Animation")]
    [SerializeField] Animation fadeAnim;

    //CheckPoint settings
    [HideInInspector]
    public Vector2 checkPointPos;
    [HideInInspector]
    public int currentCheckPoint = 0;
    [HideInInspector]
    public int checkPointRoom;

    [HideInInspector]
    public int deathCounter;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        ladyBugLight = GetComponent<LadyBugLight>();
        rb = GetComponent<Rigidbody2D>();

        checkPointPos = transform.position;

    }

    public IEnumerator Respawn()
    {
        respawning = true;
        rb.velocity = Vector2.zero;
        deathCounter++;

        //Animation de mort

        //Animation de fade
        fadeAnim.Play("FadeIn");

        yield return new WaitForSeconds(1);


        if (GameObject.FindObjectsOfType<FallingObject>().Length >= 1)
        {
            if (GameObject.FindObjectsOfType<FallingObject>().Length == 1)
            {
                GameObject.FindObjectOfType<FallingObject>().ResetFall();
            }
            else
            {
                FallingObject[] fall = GameObject.FindObjectsOfType<FallingObject>();
                for (int i = 0; i < fall.Length; i++)
                {
                    fall[i].ResetFall();
                }
            }
        }
        if(levelManager != null)
            levelManager.currentRoom = checkPointRoom;

        transform.position = checkPointPos;
        virtualCamera.transform.position = transform.position;
        //Reset Light
        ladyBugLight.lightActive = false;
        ladyBugLight.ladyLight.intensity = 0;
        ladyBugLight.ladyLight.enabled = false;

        //RespawnAphid
        if (GameObject.FindObjectsOfType<AphidCollect>().Length >= 1)
        {
            if (GameObject.FindObjectsOfType<AphidCollect>().Length == 1)
            {
                GameObject.FindObjectOfType<AphidCollect>().RespawnAphidWhenDead();
            }
            else
            {
                AphidCollect[] aphids = GameObject.FindObjectsOfType<AphidCollect>();
                for (int i = 0; i < aphids.Length; i++)
                {
                    aphids[i].RespawnAphidWhenDead();
                }
            }
        }

        yield return new WaitForSeconds(1);
        //animation de fade
        fadeAnim.Play("FadeOut");
        fadeAnim.Play("FadeOut");

        respawning = false;
    }
}
