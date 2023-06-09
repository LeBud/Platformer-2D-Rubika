using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour
{

    [Header("Loading Screen")]
    [SerializeField] Image loadingBar;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] TextMeshProUGUI loadingTxt;

    ParallaxSwap parallaxSwap;

    private void Start()
    {
        parallaxSwap = FindObjectOfType<ParallaxSwap>();
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

    public IEnumerator PlayerCanMove(int i)
    {
        if (i == 0)
            PlayerController.canMove = false;
        else
            PlayerController.canMove = true;

        yield return null;
    }

    void Transform()
    {
        GetComponent<PlayerController>().transform.position = new Vector2(783, -95);
        parallaxSwap.parallax = ParallaxSwap.Parallax.termiteParallax;
        parallaxSwap.ParallaxSwapFonction();
    }
}
