using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    PlayerController playerController;
    PlayerDeath playerDeath;
    LadyBugLight ladyLight;

    [Header("UI Elements")]
    [SerializeField] Slider glideSlider;
    [SerializeField] Slider lightSlider;
    [SerializeField] TextMeshProUGUI deathCounterTxt;
    [SerializeField] TextMeshProUGUI collectablesTxt;
    [SerializeField] float glideSliderYOffset;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerDeath = GetComponent<PlayerDeath>();
        ladyLight = GetComponent<LadyBugLight>();
        lightSlider.maxValue = playerController.playerControllerData.maxAphid * 10;
        glideSlider.maxValue = playerController.playerControllerData.maxGlideTime;
        deathCounterTxt.text = "Death : " + playerDeath.deathCounter;
    }

    private void LateUpdate()
    {
        if (playerController.gliding)
            glideSlider.gameObject.SetActive(true);
        else
            glideSlider.gameObject.SetActive(false);

        Vector2 playerPos = Camera.main.WorldToScreenPoint(transform.position);

        playerPos.y += glideSliderYOffset;

        glideSlider.transform.position = playerPos;


        glideSlider.value = playerController.glideTime;
        lightSlider.value = ladyLight.aphidCount * 10;
        deathCounterTxt.text = "Death : " + playerDeath.deathCounter;
        collectablesTxt.text = "Collectable : " + PlayerCollectable.collectable;
    }

}
