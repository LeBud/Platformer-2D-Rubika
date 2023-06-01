using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LadyBugLight : MonoBehaviour
{
    NewAchievementSystem achievements;
    PlayerControllerData playerControllerData;
    PlayerController controller;

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
        achievements = FindObjectOfType<NewAchievementSystem>();
        playerControllerData = GetComponent<PlayerController>().playerControllerData;
        controller= GetComponent<PlayerController>();
        ladyLight.intensity = 0;
        ladyLight.enabled = false;

        if(!oldSystem)
            ladyLight.enabled = true;
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

        currentLightLevel = Mathf.Clamp(currentLightLevel, 0, 2);
        if(!oldSystem)
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
            if (input > .0001f && aphidCharge > 0)
                inputing = true;
            else
                inputing = false;
        }
    }


    void LightSystem()
    {

        if (!controller.blueAnimator)
        {
            currentLightLevel = 0;
            return;
        }

        if (inputing)
        {
            if (currentLightLevel < 2)
                currentLightLevel += Time.deltaTime * playerControllerData.timeToLightMult;
            

            aphidCharge -= Time.deltaTime * playerControllerData.lightConsMult;

        }
        else if(!inputing)
        {
            if(controller.blueAnimator && !inputing && currentLightLevel < .5f)
            {
                currentLightLevel += Time.deltaTime * playerControllerData.timeToLightMult;
            }

            if (currentLightLevel > .5f)
            {
                currentLightLevel -= Time.deltaTime * playerControllerData.timeToLightMult;
            }
        }
    }

    //OldSystem

    void LadyLight()
    {
        lightActive = true;
        aphidCount--;
        ladyLight.intensity = 2;
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

        if (ladyLight.intensity > 2) ladyLight.intensity = 2;
        
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
