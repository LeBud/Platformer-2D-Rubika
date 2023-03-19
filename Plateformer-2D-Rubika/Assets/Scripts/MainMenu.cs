using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("First Selected Button in each Menu")]
    [SerializeField] GameObject playBtt, optionsBtt, creditsBtt;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(playBtt);
    }

    public void Play(int sceneNum)
    {
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
