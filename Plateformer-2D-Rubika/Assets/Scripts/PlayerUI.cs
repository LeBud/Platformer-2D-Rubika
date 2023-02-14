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
    [SerializeField] TextMeshProUGUI aphidTxt;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        flySlider.maxValue = playerController.playerControllerData.flyMaxTime;
        glideSlider.maxValue = playerController.playerControllerData.maxGlideTime;
    }

    private void LateUpdate()
    {
        glideSlider.value = playerController.glideTime;
        flySlider.value = playerController.flyTime;
        aphidTxt.text = "consommable : " + playerController.playerControllerData.aphidAmount;
    }
}
