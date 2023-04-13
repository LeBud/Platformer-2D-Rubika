using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPause = false;
    public GameObject inGameHUD;
    public GameObject pauseMenuHUD;
    public GameObject optionsMenuHUD;

    [SerializeField] GameObject firstButton;

    private void Start()
    {
        if (gameIsPause)
            Pause();
        else
            Resume();

        EventSystem.current.SetSelectedGameObject(null);

    }

    void Update()
    {
        if (PlayerDeath.respawning) return;

        if (Input.GetButtonDown("Cancel"))
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
        optionsMenuHUD.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }

    void Pause()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inGameHUD.SetActive(false);
        pauseMenuHUD.SetActive(true);
        optionsMenuHUD.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);

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

    public void SelectBtt(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
}
