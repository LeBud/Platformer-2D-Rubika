using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsDisplay : MonoBehaviour
{

    [Header("References")]
    //[SerializeField] TextMeshProUGUI titleTxt;
    //[SerializeField] TextMeshProUGUI descriptionTxt;
    //[SerializeField] Image achievementIMG;
    //[SerializeField] Animator animator;
    [SerializeField] GameObject prefab;
    [SerializeField] Transform UI;
    Sprite sp;


    public void DisplayAchievement(string title, string description, Sprite sprite)
    {

        GameObject achievement = Instantiate(prefab, UI);
        achievement.transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = title;
        achievement.transform.GetChild(0).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = description;
        achievement.transform.GetChild(0).transform.GetChild(1).GetComponent<Image>().sprite = sprite;

        Destroy(achievement, 6);

        StartCoroutine(AnimateDisplay(achievement));
    }

    IEnumerator AnimateDisplay(GameObject achi)
    {
        achi.transform.GetChild(0).GetComponent<Animator>().Play("SlideOn");

        yield return new WaitForSeconds(5);

        achi.transform.GetChild(0).GetComponent<Animator>().Play("SlideOff");
    }

}
