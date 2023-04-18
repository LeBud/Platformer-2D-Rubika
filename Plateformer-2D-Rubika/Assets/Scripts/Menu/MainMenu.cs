using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("First Selected Button in each Menu")]
    [SerializeField] GameObject playBtt, optionsBtt, creditsBtt, continueBtt, newGameBtt;

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
        SceneManager.LoadScene(sceneNum);
    }

    public void ContinueGame(int sceneNum)
    {
        loadSave = true;
        ereaseSave = false;
        SceneManager.LoadScene(sceneNum);
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
}
