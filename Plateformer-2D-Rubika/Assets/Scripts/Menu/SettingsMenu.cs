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
    [SerializeField] TMP_Dropdown qualityDropDown;
    [SerializeField] Toggle fullscreenToggle;

    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider musicSlider;

    [Header("Text")]
    [SerializeField] TMP_Text masterVolumeTxt;
    [SerializeField] TMP_Text SFXVolumeTxt;
    [SerializeField] TMP_Text musicVolumeTxt;

    float masterSound;
    float SFXSound;
    float musicSound;

    private void Awake()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/Settings.json"))
            LoadSettings();
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
        SaveSettings();
    }

    public void Resolution(int resoltionIndex)
    {
        Resolution resolution = resolutions[resoltionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        SaveSettings();
    }

    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        SaveSettings();
    }

    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);

        float displayNumber = sliderValue * 100;
        masterVolumeTxt.text = "Master : " + displayNumber.ToString("F0");

        masterSound = sliderValue;
        SaveSettings();
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);

        float displayNumber = sliderValue * 100;
        SFXVolumeTxt.text = "SFX : " + displayNumber.ToString("F0");

        SFXSound = sliderValue;
        SaveSettings();

    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);

        float displayNumber = sliderValue * 100;
        musicVolumeTxt.text = "music : " + displayNumber.ToString("F0");

        musicSound = sliderValue;
        SaveSettings();

    }


    void SaveSettings()
    {

        SavedSettings savedSettings = new SavedSettings
        {
            isFullscreen = Screen.fullScreen,
            qualityLevel = QualitySettings.GetQualityLevel(),
            resolutionWidth = Screen.currentResolution.width,
            resolutionHeight = Screen.currentResolution.height,
            resolutionNum = resolutionDropDown.value,
            masterAudio = masterSound,
            SFXAudio = SFXSound,
            musicAudio = musicSound,
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
        Screen.SetResolution(savedSettings.resolutionWidth, savedSettings.resolutionHeight, Screen.fullScreen);

        //Audio Mixer Load
        audioMixer.SetFloat("Master", Mathf.Log10(savedSettings.masterAudio) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(savedSettings.SFXAudio) * 20);
        audioMixer.SetFloat("Music", Mathf.Log10(savedSettings.musicAudio) * 20);

        masterVolumeTxt.text = "Master : " + (savedSettings.masterAudio * 100).ToString("F0");
        SFXVolumeTxt.text = "SFX : " + (savedSettings.SFXAudio * 100).ToString("F0");
        musicVolumeTxt.text = "music : " + (savedSettings.musicAudio * 100).ToString("F0");

        masterSlider.value = savedSettings.masterAudio;
        SFXSlider.value = savedSettings.SFXAudio;
        musicSlider.value = savedSettings.musicAudio;

        qualityDropDown.value = savedSettings.qualityLevel;

        CheckAllRes();

        resolutionDropDown.value = savedSettings.resolutionNum;

        fullscreenToggle.isOn = savedSettings.isFullscreen;
    }

}

public class SavedSettings
{
    public bool isFullscreen;
    public int qualityLevel;
    public int resolutionWidth;
    public int resolutionHeight;
    public int resolutionNum;
    public float masterAudio;
    public float SFXAudio;
    public float musicAudio;
}
