using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{

    [SerializeField] Animator loadingScreen;
    [SerializeField] Animator FallAnim;
    [SerializeField] Animator EndGameAnim;
    
    public bool fallAnim, endAnim;

    void Start()
    {
        loadingScreen.Play("LoadingScreenFadeOut");
    }


    public void FallAnimation()
    {

    }

    public void EndAnimation()
    {
        EndGameAnim.Play("EndGame");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (endAnim)
                EndAnimation();
            else if(fallAnim)
                FallAnimation();
        }
    }

}
