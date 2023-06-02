using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPause = false;
    public GameObject inGameHUD;
    public GameObject pauseMenuHUD;
    public GameObject optionsMenuHUD;

    [HideInInspector]
    public InputController inputController;
    [HideInInspector]
    public InputAction pauseBtt, saveBtt, loadBtt;

    [SerializeField] GameObject firstButton;

    private void Awake()
    {
        inputController = new InputController();
    }

    private void OnEnable()
    {
        inputController.Enable();
        pauseBtt = inputController.UI.Pause;
        saveBtt= inputController.UI.Save;
        loadBtt= inputController.UI.Load;
    }

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

        if (pauseBtt.WasPressedThisFrame())
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

        FindObjectOfType<SettingsMenu>().SaveSettings();
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
