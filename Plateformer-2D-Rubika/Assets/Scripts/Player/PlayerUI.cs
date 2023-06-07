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
    GameManager gameManager;

    [Header("UI Elements")]
    [SerializeField] Slider glideSlider;
    [SerializeField] Slider lightSlider;
    [SerializeField] TextMeshProUGUI deathCounterTxt;
    [SerializeField] TextMeshProUGUI collectablesTxt;
    [SerializeField] float glideSliderYOffset;

    Animator slider;
    Animator collectable;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerController = GetComponent<PlayerController>();
        playerDeath = GetComponent<PlayerDeath>();
        ladyLight = GetComponent<LadyBugLight>();
        
        if(ladyLight.oldSystem)
            lightSlider.maxValue = playerController.playerControllerData.maxAphid * 10;
        else
            lightSlider.maxValue = playerController.playerControllerData.maxAphidCharge;
        
        glideSlider.maxValue = playerController.playerControllerData.maxGlideTime;
        
        deathCounterTxt.text = "Death : " + playerDeath.deathCounter;

        collectable = collectablesTxt.GetComponent<Animator>();
        slider = lightSlider.GetComponent<Animator>();
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
        
        if(ladyLight.oldSystem)
            lightSlider.value = ladyLight.aphidCount * 10;
        else
            lightSlider.value = ladyLight.aphidCharge;

        deathCounterTxt.text = "Death : " + playerDeath.deathCounter;
        collectablesTxt.text = "Collectable : " + gameManager.collectableCount;
        
        if (playerController.blueAnimator && lightSlider.transform.GetChild(0).GetComponent<Image>().color.a == 0)
        {
            slider.Play("lightSliderOn");
        }
        else if (!playerController.blueAnimator && lightSlider.transform.GetChild(0).GetComponent<Image>().color.a == 1)
        {
            slider.Play("lightSliderOff");
        }

    }

    public IEnumerator ShowCollectable()
    {
        if(collectablesTxt.color.a == 0)
            collectable.Play("CollectableOn");

        yield return new WaitForSeconds(1);

        collectable.Play("CollectableOff");
    }

}
