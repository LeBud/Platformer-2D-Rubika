using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] TMP_Text masterVolumeTxt;
    [SerializeField] TMP_Text SFXVolumeTxt;
    [SerializeField] TMP_Text musicVolumeTxt;
    public string masterVolume;
    public string SFXVolume;
    public string musicVolume;

    float displayNumber;

    private void Awake()
    {
        CheckAllRes();
        LoadSettings();

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

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);

        displayNumber = sliderValue * 100;
        masterVolumeTxt.text = displayNumber.ToString("F0");
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);

        displayNumber = sliderValue * 100;
        SFXVolumeTxt.text = displayNumber.ToString("F0");
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);

        displayNumber = sliderValue * 100;
        musicVolumeTxt.text = displayNumber.ToString("F0");
    }


    void SaveSettings()
    {

        SavedSettings savedSettings = new SavedSettings
        {
            isFullscreen = Screen.fullScreen,
            qualityLevel = QualitySettings.GetQualityLevel(),
            resolution = Screen.currentResolution,
        };

        string jsonData = JsonUtility.ToJson(savedSettings);
        string filePath = Application.persistentDataPath+ "/Settings.json";

        System.IO.File.WriteAllText(filePath, jsonData);

    }

    void LoadSettings()
    {
        string filePath = Application.persistentDataPath + "/Settings.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedSettings savedSettings = JsonUtility.FromJson<SavedSettings>(jsonData);

        Screen.fullScreen = savedSettings.isFullscreen;
        QualitySettings.SetQualityLevel(savedSettings.qualityLevel);
        Screen.SetResolution(savedSettings.resolution.width, savedSettings.resolution.height, Screen.fullScreen);
    }

}

public class SavedSettings
{
    public bool isFullscreen;
    public int qualityLevel;
    public Resolution resolution;
    public float masterAudio;
    public float SFXAudio;
    public float musicAudio;
}
