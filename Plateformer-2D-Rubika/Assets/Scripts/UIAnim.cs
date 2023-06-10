using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{

    [SerializeField] Animator loadingScreen;
    [SerializeField] Animator FallAnim;
    [SerializeField] Animator EndGameAnim;
    [SerializeField] Animator ExitAnim;

    public bool fallAnim, endAnim, exitAnim, isLoadingScreen;

    void Start()
    {
        if (isLoadingScreen)
        {
            loadingScreen.gameObject.SetActive(true);
            loadingScreen.Play("LoadingScreenFadeOut");
        }
    }
    public void FallAnimation()
    {
        FallAnim.Play("Cinematique");
    }

    public void EndAnimation()
    {
        EndGameAnim.Play("EndGame");
    }

    public void ExitAnimation()
    {
        ExitAnim.Play("Exit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
        if (endAnim)
            EndAnimation();
        else if (fallAnim)
            FallAnimation();
        else if (exitAnim)
            ExitAnimation();
        }
    }

}
