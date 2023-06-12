using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Loading Screen")]
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextMeshProUGUI loadingTxt;


    private void Awake()
    {
        inputController = new InputController();
        FindObjectOfType<SettingsMenu>().LoadSettings();
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
        /*if (gameIsPause)
            Pause();
        else
            Resume();*/

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

        StartCoroutine(LoadSceneAsync(0));
    }

    public IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        PlayerController.canMove = false;
        FindObjectOfType<SaveSystem>().SaveData();

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.fillAmount = progress;
            loadingTxt.text = "Loading progress : " + "\n" + (progress * 100).ToString("F0") + "%";
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }

    public void SelectBtt(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
}
