using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDoorController : MonoBehaviour
{

    public enum RightExit {blueLevel, outchLevel, gardenLevel };
    public enum LeftExit { blueLevel, outchLevel, gardenLevel };

    [Header("Change Animations")]
    public RightExit rightExit;
    public LeftExit leftExit;

    [Header("Glide Control")]
    public bool canGlide = true;

    [Header("Parallax")]
    public bool swapParallax;
    public ParallaxSwap.Parallax parallaxRight;
    public ParallaxSwap.Parallax parallaxLeft;

    PlayerController playerController;
    ParallaxSwap parallaxSwap;
    Collider2D coll;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        coll= GetComponent<Collider2D>();
        parallaxSwap = FindObjectOfType<ParallaxSwap>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Vector2 exitDirection = (collision.transform.position - coll.bounds.center).normalized;

            if(exitDirection.x > 0 )
            {
                if(rightExit == RightExit.blueLevel)
                {
                    playerController.blueAnimator = true;
                    playerController.gardenAnimator = false;
                }
                else if(rightExit == RightExit.gardenLevel)
                {
                    playerController.blueAnimator = false;
                    playerController.gardenAnimator = true;
                }
                else
                {
                    playerController.blueAnimator = false;
                    playerController.gardenAnimator = false;
                }

                if(swapParallax)
                    parallaxSwap.parallax = parallaxRight;
            }

            if(exitDirection.x < 0 )
            {
                if (leftExit == LeftExit.blueLevel)
                {
                    playerController.blueAnimator = true;
                    playerController.gardenAnimator = false;
                }
                else if (leftExit == LeftExit.gardenLevel)
                {
                    playerController.blueAnimator = false;
                    playerController.gardenAnimator = true;
                }

                else
                {
                    playerController.blueAnimator = false;
                    playerController.gardenAnimator = false;
                }

                if(swapParallax)
                    parallaxSwap.parallax = parallaxLeft;

            }



            if (canGlide)
                playerController.canGlide = true;
        }
    }

}
