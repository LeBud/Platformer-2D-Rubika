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
    AchievementsCheck achievements;

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


    FallingObject[] fallObjects;
    AphidCollect[] aphid;
    MantysEnablerDisabler[] mantisEnablers;

    private void Awake()
    {
        respawning = false;

        levelManager = FindObjectOfType<LevelManager>();
        achievements = FindObjectOfType<AchievementsCheck>();
        ladyBugLight = GetComponent<LadyBugLight>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        checkPointPos = transform.position;
    }

    private void Start()
    {
        if(GameObject.FindObjectsOfType<FallingObject>().Length >= 1)
        {
            fallObjects = GameObject.FindObjectsOfType<FallingObject>();
        }

        if (GameObject.FindObjectsOfType<AphidCollect>().Length >= 1)
        {
            aphid = GameObject.FindObjectsOfType<AphidCollect>();
        }

        if(GameObject.FindObjectsOfType<MantysEnablerDisabler>().Length >= 1)
        {
            mantisEnablers = GameObject.FindObjectsOfType<MantysEnablerDisabler>();
        }
    }

    public IEnumerator Respawn()
    {
        respawning = true;
        rb.simulated = false;
        rb.velocity = Vector2.zero;
        deathCounter++;

        achievements.deathCount = deathCounter;

        FindObjectOfType<SaveSystem>().SaveDeath();

        //Animation de mort

        yield return new WaitForEndOfFrame();
        
        fadeAnim.Play("FadeIn");

        yield return new WaitForSeconds(.5f);

        CameraManager.instance.ResetCam();

        if(mantisEnablers != null)
        {
            foreach(var obj in mantisEnablers)
            {
                obj.used = false;
            }
        }

        if(MantysEnablerDisabler.mantisEnable)
        {
            if (FindObjectOfType<MantisAI>())
            {
                MantisAI mantis = FindObjectOfType<MantisAI>();
                StartCoroutine(mantis.Respawn());
            }
                
        }

        playerController.inAirFlow = false;
        playerController.gliding = false;
        playerController.glideTime = 0;

        #region FallObjectReset
        /*if (GameObject.FindObjectsOfType<FallingObject>().Length >= 1)
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
        }*/

        if(fallObjects != null)
        {
            foreach(var obj in fallObjects)
            {
                obj.ResetFall();
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
        /*if (GameObject.FindObjectsOfType<AphidCollect>().Length >= 1)
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
        }*/
        #endregion


        FindObjectOfType<SaveSystem>().LoadData();

        rb.velocity = Vector2.zero;
        rb.simulated = true;

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
