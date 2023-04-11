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
    PlayerController playerController;

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
    public bool lightCheckPoint;

    [HideInInspector]
    public int deathCounter;

    private void Awake()
    {
        respawning = false;

        levelManager = FindObjectOfType<LevelManager>();
        ladyBugLight = GetComponent<LadyBugLight>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        checkPointPos = transform.position;

    }

    public IEnumerator Respawn()
    {
        respawning = true;
        rb.velocity = Vector2.zero;
        deathCounter++;

        FindObjectOfType<SaveSystem>().SaveDeath();

        //Animation de mort

        fadeAnim.Play("FadeIn");

        yield return new WaitForSeconds(.5f);

        if(MantysEnablerDisabler.mantisEnable)
        {
            MantysFollower mantis = FindObjectOfType<MantysFollower>();
            StartCoroutine(mantis.RespawnEnemy());
        }

        playerController.inAirFlow = false;
        playerController.gliding = false;
        playerController.glideTime = 0;

        #region FallObjectReset
        if (GameObject.FindObjectsOfType<FallingObject>().Length >= 1)
        {
            if (GameObject.FindObjectsOfType<FallingObject>().Length == 1)
            {
                StartCoroutine(GameObject.FindObjectOfType<FallingObject>().ResetFall());
            }
            else
            {
                FallingObject[] fall = GameObject.FindObjectsOfType<FallingObject>();
                for (int i = 0; i < fall.Length; i++)
                {
                    StartCoroutine(fall[i].ResetFall());
                }
            }
        }
    #endregion

        if (levelManager != null)
            levelManager.currentRoom = checkPointRoom;

        transform.position = checkPointPos;
        virtualCamera.transform.position = transform.position;
        
        //Reset Light
        ladyBugLight.lightActive = false;
        ladyBugLight.ladyLight.intensity = 0;
        ladyBugLight.ladyLight.enabled = false;

        #region Aphid
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
        #endregion

        
        FindObjectOfType<SaveSystem>().LoadData();

        if (lightCheckPoint && ladyBugLight.aphidCount < 1)
            ladyBugLight.aphidCount = 1;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(.5f);

        fadeAnim.Play("FadeOut");

        yield return new WaitForSeconds(.4f);

        respawning = false;
    }
}
