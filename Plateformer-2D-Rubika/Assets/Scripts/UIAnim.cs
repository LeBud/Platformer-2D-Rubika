using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{

    [SerializeField] Animator loadingScreen;
    

    void Start()
    {
        loadingScreen.Play("LoadingScreenFadeOut");
    }


    public void FallAnimation()
    {

    }

}
