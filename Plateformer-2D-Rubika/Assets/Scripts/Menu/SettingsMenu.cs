using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    Resolution[] resolutions;

    [Header("Graphics")]
    [SerializeField] TMP_Dropdown resolutionDropDown;

    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TMP_Text volume;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider musicSlider;
    public string masterVolume;
    public string SFXVolume;
    public string musicVolume;

    float displayNumber;

    private void Awake()
    {
     
        CheckAllRes();

        masterSlider.onValueChanged.AddListener(SetVolume);
        SFXSlider.onValueChanged.AddListener(SetVolume);
        musicSlider.onValueChanged.AddListener(SetVolume);

    }

    void CheckAllRes()
    {
        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolution = 0;


        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " / " + resolutions[i].refreshRateRatio + " hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolution;
        resolutionDropDown.RefreshShownValue();
    }

    public void Graphics(int graphicsIndex)
    {
        QualitySettings.SetQualityLevel(graphicsIndex);
    }

    public void Resolution(int resoltionIndex)
    {
        Resolution resolution = resolutions[resoltionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);

        displayNumber = sliderValue * 100;
        //volume.text = displayNumber.ToString("F0");
    }

}
