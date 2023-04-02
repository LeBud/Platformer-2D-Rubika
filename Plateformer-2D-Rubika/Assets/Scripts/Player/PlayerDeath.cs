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

        //Animation de mort

        fadeAnim.Play("FadeIn");

        yield return new WaitForSeconds(.5f);

        playerController.inAirFlow = false;
        playerController.gliding = false;
        playerController.glideTime = 0;

        #region FallObjectReset
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

        #region collectable
        if (GameObject.FindObjectsOfType<Collectable>().Length >= 1)
        {
            if (GameObject.FindObjectsOfType<Collectable>().Length == 1)
            {
                if (!GameObject.FindObjectOfType<Collectable>().checkPointSave)
                {
                    Collectable collect = GameObject.FindObjectOfType<Collectable>();

                    collect.GetComponent<SpriteRenderer>().enabled = true;
                    collect.GetComponent<CircleCollider2D>().enabled = true;

                    PlayerCollectable.collectable--;
                }
            }
            else
            {
                Collectable[] collect = GameObject.FindObjectsOfType<Collectable>();
                for (int i = 0; i < collect.Length; i++)
                {
                    if (!collect[i].checkPointSave)
                    {
                        collect[i].GetComponent<SpriteRenderer>().enabled = true;
                        collect[i].GetComponent<CircleCollider2D>().enabled = true;

                        PlayerCollectable.collectable--;
                    }
                }
            }
        }
        #endregion

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
