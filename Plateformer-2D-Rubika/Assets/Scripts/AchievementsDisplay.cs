using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsDisplay : MonoBehaviour
{

    [Header("References")]
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] TextMeshProUGUI descriptionTxt;
    [SerializeField] Image achievementIMG;
    [SerializeField] Animator animator;


    public void DisplayAchievement(string title, string description, Sprite sprite)
    {
        titleTxt.text = title;
        descriptionTxt.text = description;
        achievementIMG.sprite = sprite;

        StartCoroutine(AnimateDisplay());
    }

    IEnumerator AnimateDisplay()
    {
        animator.Play("ShowUp");

        yield return new WaitForSeconds(5);

        animator.Play("ShowDown");
    }

}
