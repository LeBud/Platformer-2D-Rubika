using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LadyBugLight : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] PlayerControllerData playerControllerData;
    public int aphidCount;
    public Light2D ladyLight;

    [HideInInspector]
    public bool lightActive;

    private void Awake()
    {
        ladyLight.intensity = 0;
        ladyLight.enabled = false;
    }

    private void Update()
    {
        MyInputs();

    }

    void MyInputs()
    {
        float input = Input.GetAxisRaw("Fire1");

        if (input > .0001f && aphidCount > 0 && !lightActive)
        {
            LadyLight();
        }
    }

    void LadyLight()
    {
        lightActive = true;
        aphidCount--;
        ladyLight.intensity = 0;
        ladyLight.enabled = true;
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
