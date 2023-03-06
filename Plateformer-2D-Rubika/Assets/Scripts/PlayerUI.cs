using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    PlayerController playerController;

    [Header("UI Elements")]
    [SerializeField] Slider glideSlider;
    [SerializeField] Slider flySlider;
    [SerializeField] TextMeshProUGUI deathCounterTxt;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        flySlider.maxValue = playerController.playerControllerData.flyMaxTime;
        glideSlider.maxValue = playerController.playerControllerData.maxGlideTime;
        deathCounterTxt.text = "Death : " + playerController.deathCounter;
    }

    private void LateUpdate()
    {
        if (playerController.gliding)
            glideSlider.gameObject.SetActive(true);
        else
            glideSlider.gameObject.SetActive(false);

        Vector2 playerPos = playerController.gameObject.transform.position;

        playerPos.y += 2;

        glideSlider.gameObject.transform.position = playerPos;

        glideSlider.value = playerController.glideTime;
        flySlider.value = playerController.flyTime;
        deathCounterTxt.text = "Death : " + playerController.deathCounter;
    }
}
