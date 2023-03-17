using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPause = false;
    public GameObject inGameHUD;
    public GameObject pauseMenuHUD;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            gameIsPause = !gameIsPause;

            if (gameIsPause)
                Pause();
            else
                Resume();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inGameHUD.SetActive(true);
        pauseMenuHUD.SetActive(false);
    }

    void Pause()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inGameHUD.SetActive(false);
        pauseMenuHUD.SetActive(true);
    }

    public void Option()
    {
        Debug.Log("Loading Option");
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        gameIsPause = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(0);
    }
}
