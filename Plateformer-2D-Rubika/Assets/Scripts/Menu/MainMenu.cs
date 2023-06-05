using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("First Selected Button in each Menu")]
    [SerializeField] GameObject playBtt, optionsBtt, creditsBtt, continueBtt, newGameBtt;

    [Header("Loading Screen")]
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextMeshProUGUI loadingTxt;

    public static bool loadSave;
    public static bool ereaseSave;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playBtt);

        if (System.IO.File.Exists(Application.persistentDataPath + "/SaveFile.json"))
        {
            continueBtt.GetComponent<Button>().interactable = true;
            playBtt.GetComponent<Button>().onClick.AddListener(delegate {SelectBtt(continueBtt); });
        }
        else
        {
            continueBtt.GetComponent<Button>().interactable = false;
            playBtt.GetComponent<Button>().onClick.AddListener(delegate { SelectBtt(newGameBtt); });
        }

    }

    public void NewGame(int sceneNum)
    {
        ereaseSave = true;
        loadSave = false;
        StartCoroutine(LoadSceneAsync(sceneNum));
    }

    public void ContinueGame(int sceneNum)
    {
        loadSave = true;
        ereaseSave = false;
        StartCoroutine(LoadSceneAsync(sceneNum));
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SelectBtt(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }


    IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.fillAmount = progress;
            loadingTxt.text = "Loading progress : " + "\n" + (progress * 100).ToString("F0") + "%";
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }
}
