using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Play(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }


    public void Exit()
    {
        Application.Quit();
    }
}
