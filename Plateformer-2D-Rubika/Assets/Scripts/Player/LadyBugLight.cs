using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LadyBugLight : MonoBehaviour
{
    AchievementsCheck achievements;
    PlayerControllerData playerControllerData;

    [HideInInspector]
    public bool lightActive;

    [Header("Settings")]
    public int aphidCount;
    public Light2D ladyLight;


    [Header("New Settings")]
    public float aphidCharge;
    public bool oldSystem;
    bool inputing;

    float currentLightLevel;

    private void Start()
    {
        achievements = FindObjectOfType<AchievementsCheck>();
        playerControllerData = GetComponent<PlayerController>().playerControllerData;

        ladyLight.intensity = 0;
        ladyLight.enabled = false;
    }

    private void Update()
    {
        if (PlayerDeath.respawning)
        {
            StopAllCoroutines();
            return;
        }
        if (PauseMenu.gameIsPause) return;

        MyInputs();

        LightSystem();

        currentLightLevel = Mathf.Clamp(currentLightLevel, 0, 1);
        ladyLight.intensity = currentLightLevel;
    }

    void MyInputs()
    {
        float input = Input.GetAxisRaw("Fire1");

        if (oldSystem)
        {
            if (input > .0001f && aphidCount > 0 && !lightActive)
                LadyLight();
        }
        else
        {
            if (input > .0001f && aphidCharge > 0 && !lightActive)
                inputing = true;
            else
                inputing = false;
        }
    }


    void LightSystem()
    {
        if (inputing && aphidCharge > 0)
        {
            while(currentLightLevel < 1)
            {
                currentLightLevel += Time.deltaTime * playerControllerData.timeToLightMult;
            }
            

            aphidCharge -= Time.deltaTime * playerControllerData.lightConsMult;

        }
        else
        {
            while (currentLightLevel > 0)
            {
                currentLightLevel += Time.deltaTime * playerControllerData.timeToLightMult;
            }
        }
    }

    //OldSystem

    void LadyLight()
    {
        lightActive = true;
        aphidCount--;
        ladyLight.intensity = 0;
        ladyLight.enabled = true;

        achievements.aphidUse = true;

        StartCoroutine(LightOn());
    }




    IEnumerator LightOn()
    {
        for (int i = 0; i < 20; i++)
        {
            ladyLight.intensity += .05f;
            yield return new WaitForSeconds(.04f);
        }

        if (ladyLight.intensity > 1) ladyLight.intensity = 1;
        
        yield return new WaitForSeconds(playerControllerData.maxTimeLightOn - .7f);


        StartCoroutine(FadeLight());
    }

    IEnumerator FadeLight()
    {
        for(int x = 0; x < 3; x++)
        {
            for (int i = 0; i < 20; i++)
            {
                ladyLight.intensity -= .05f;
                yield return new WaitForSeconds(.005f);
            }

            for (int i = 0; i < 20; i++)
            {
                ladyLight.intensity += .05f;
                yield return new WaitForSeconds(.005f);

            }
        }

        for (int i = 0; i < 20; i++)
        {
            ladyLight.intensity -= .05f;
            yield return new WaitForSeconds(.005f);
        }

        lightActive = false;
        ladyLight.enabled = false;

    }
}
