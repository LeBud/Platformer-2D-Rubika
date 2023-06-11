using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{

    [SerializeField] Animator loadingScreen;
    [SerializeField] Animator cinematics;

    public bool fallAnim, endAnim, exitAnim;

    void Start()
    {
        if(loadingScreen != null)
            loadingScreen.Play("LoadingScreenFadeOut");
    }

    public void FallAnimation()
    {
        cinematics.Play("Cinematique");
    }

    public void EndAnimation()
    {
        cinematics.Play("EndGame");
    }

    public void ExitAnimation()
    {
        cinematics.Play("Exit");
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
