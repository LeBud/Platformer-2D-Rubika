using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementMenuDisplay : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Sprite lockedAchievement;
    [SerializeField] TextMeshProUGUI successTxt;
    [SerializeField] List<GameObject> achievementDisplay;
    List<AchievementsSO> achievementsList;

    NewAchievementSystem achievementSystem;

    List<string> successName = new List<string>();

    //AchievementsCheck achievements;

    private void Awake()
    {
        achievementSystem = GetComponent<NewAchievementSystem>();
        LoadAchievement();
    }

    private void Start()
    {
        achievementsList = achievementSystem.achievements;

        RefreshAchievement();
    }

    void RefreshAchievement()
    {
        for(int i = 0; i < achievementsList.Count; i++)
        {
            Debug.Log(i);

            if (achievementsList[i].achieved)
            {
                achievementDisplay[i].transform.GetChild(0).GetComponent<Image>().sprite = achievementsList[i].image;
                successName.Add(achievementsList[i].achievementName + "\n" + achievementsList[i].description);
            }
            else
            {
                achievementDisplay[i].transform.GetChild(0).GetComponent<Image>().sprite = lockedAchievement;
                successName.Add("???");
            }
        }

    }

    private void LateUpdate()
    {
        if (achievementDisplay.Contains(EventSystem.current.currentSelectedGameObject))
            successTxt.text = successName[achievementDisplay.IndexOf(EventSystem.current.currentSelectedGameObject)];
        else
            successTxt.text = "???";
    }


    public void LoadAchievement()
    {
        //Récupérer les données sauvegardées
        string filePath = Application.persistentDataPath + "/SaveFile.json";

        if (!System.IO.File.Exists(filePath))
            return;

        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        //Charger les données sauvergardées
        achievementSystem.CheckCompletion(savedData.achievementsData);

        Debug.Log("Chargement des données terminées");
    }

}
