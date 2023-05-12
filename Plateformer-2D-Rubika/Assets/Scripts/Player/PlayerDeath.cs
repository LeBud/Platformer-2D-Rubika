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

    IEnumerator RespawnPlatforms()
    {
        //FallObject
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

        //Mantis
        if (GameObject.FindObjectsOfType<MantysEnablerDisabler>().Length >= 1)
        {
            if (GameObject.FindObjectsOfType<MantysEnablerDisabler>().Length == 1)
            {
                GameObject.FindObjectOfType<MantysEnablerDisabler>().used = false;
            }
            else
            {
                MantysEnablerDisabler[] mantis = GameObject.FindObjectsOfType<MantysEnablerDisabler>();
                for (int i = 0; i < mantis.Length; i++)
                {
                    mantis[i].used = false;
                }
            }
        }

        yield return null;
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


        playerController.inAirFlow = false;
        playerController.gliding = false;
        playerController.glideTime = 0;

        StartCoroutine(RespawnPlatforms());

        if (levelManager != null)
            levelManager.currentRoom = checkPointRoom;

        transform.position = checkPointPos;
        virtualCamera.transform.position = transform.position;
        
        //Reset Light
        ladyBugLight.lightActive = false;
        ladyBugLight.ladyLight.intensity = 0;
        ladyBugLight.ladyLight.enabled = false;


        FindObjectOfType<SaveSystem>().LoadData();

        rb.velocity = Vector2.zero;
        rb.simulated = true;

        if (lightCheckPoint && ladyBugLight.aphidCount < 1)
            ladyBugLight.aphidCount = 1;

        StartCoroutine(RespawnPlatforms());

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
