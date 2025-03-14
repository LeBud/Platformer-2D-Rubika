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
    NewAchievementSystem achievements;

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

    List<FallingObject> fallingObjects = new List<FallingObject>();
    List<MantysEnablerDisabler> mantysEnablerDisablers = new List<MantysEnablerDisabler>();
    List<BreakPlatform> breakPlat = new List<BreakPlatform>();
    
    AudioSource source;

    private void Awake()
    {
        respawning = false;

        levelManager = FindObjectOfType<LevelManager>();
        achievements = FindObjectOfType<NewAchievementSystem>();
        ladyBugLight = GetComponent<LadyBugLight>();
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        checkPointPos = transform.position;

        if(FindObjectsOfType<FallingObject>().Length >= 1)
        {
            FallingObject[] fall = FindObjectsOfType<FallingObject>();
            for (int i = 0; i < fall.Length; i++)
            {
                fallingObjects.Add(fall[i]);
            }
        }

        if (FindObjectsOfType<MantysEnablerDisabler>().Length >= 1)
        {
            MantysEnablerDisabler[] mantys = FindObjectsOfType<MantysEnablerDisabler>();
            for (int i = 0; i < mantys.Length; i++)
            {
                mantysEnablerDisablers.Add(mantys[i]);
            }
        }

        if(FindObjectsOfType<BreakPlatform>().Length > 1)
        {
            BreakPlatform[] plat = FindObjectsOfType<BreakPlatform>();
            for(int i = 0; i < plat.Length; i++)
            {
                breakPlat.Add(plat[i]);
            }
        }

        source = GetComponent<AudioSource>();
    }

    IEnumerator RespawnPlatforms()
    {

        for (int i = 0; i < fallingObjects.Count; i++)
        {
            StartCoroutine(fallingObjects[i].ResetFall());
        }

        for(int i = 0; i < mantysEnablerDisablers.Count; i++)
        {
            mantysEnablerDisablers[i].used = false;
        }

        for (int i = 0; i < breakPlat.Count; i++)
        {
            breakPlat[i].ReactivePlatform();
        }

        if (FindObjectsOfType<AphidCollect>().Length > 0)
        {
            AphidCollect[] aphids = FindObjectsOfType<AphidCollect>();
            for (int i = 0; i < aphids.Length; i++)
            {
                aphids[i].RespawnAphidWhenDead();
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

        playerController.animator.SetTrigger("Death");
        source.PlayOneShot(playerController.deathSound);

        achievements.deathCount = deathCounter;

        LevelManager.respawning = true;

        FindObjectOfType<SaveSystem>().SaveDeath();

        yield return new WaitForSeconds(.25f);
        
        fadeAnim.Play("FadeIn");

        yield return new WaitForSeconds(.5f);

        levelManager.ActiveAllLevel();

        StartCoroutine(RespawnPlatforms());

        CameraManager.instance.ResetCam();

        playerController.inAirFlow = false;
        playerController.gliding = false;
        playerController.glideTime = 0;

        if (levelManager != null)
            levelManager.currentRoom = checkPointRoom;

        transform.position = checkPointPos;
        virtualCamera.transform.position = transform.position;
        
        //Reset Light
        ladyBugLight.lightActive = false;
        ladyBugLight.ladyLight.intensity = 0;
        //ladyBugLight.ladyLight.enabled = false;

        FindObjectOfType<SaveSystem>().LoadData();

        rb.velocity = Vector2.zero;
        rb.simulated = true;

        if (lightCheckPoint && ladyBugLight.aphidCharge < 100)
            ladyBugLight.aphidCharge = 100;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(.5f);

        StartCoroutine(RespawnPlatforms());

        fadeAnim.Play("FadeOut");
        playerController.animator.SetTrigger("Respawn");
        yield return new WaitForSeconds(.4f);

        LevelManager.respawning = false;

        respawning = false;
    }
}
