using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAchievementSystem : MonoBehaviour
{
    AchievementsDisplay display;
    GameManager manager;

    public List<AchievementsSO> achievements;
    public static List<AchievementData> achievementsData;

    [HideInInspector]
    public int jumpCount, deathCount, collectableCount;

    [HideInInspector]
    public bool airFlowUse, jumpPadUse, breakPlatformUse, aphidUse, fallObjectUse, fallInCatacomb, surviveChase, exitCatacomb, endGame;

    private void Start()
    {
        display = FindObjectOfType<AchievementsDisplay>();
        manager = GetComponent<GameManager>();
        RefreshData();
        if(manager != null)
            collectableCount = manager.collectableCount;
    }

    public void UpdateValue()
    {
        //float achievement
        achievements[0].actualValue = collectableCount;
        achievements[1].actualValue = collectableCount;
        achievements[2].actualValue = collectableCount;
        achievements[3].actualValue = deathCount;
        achievements[4].actualValue = deathCount;
        achievements[5].actualValue = jumpCount;
        achievements[6].actualValue = jumpCount;
        achievements[7].actualValue = airFlowUse ? 1 : 0;
        achievements[8].actualValue = aphidUse ? 1 : 0;
        achievements[9].actualValue = breakPlatformUse ? 1 : 0;
        achievements[10].actualValue = fallObjectUse ? 1 : 0;
        achievements[11].actualValue = jumpPadUse ? 1 : 0;
        achievements[12].actualValue = fallInCatacomb ? 1 : 0;
        achievements[13].actualValue = surviveChase ? 1 : 0;
        achievements[14].actualValue = exitCatacomb ? 1 : 0;
        achievements[15].actualValue = endGame ? 1 : 0;
    }

    private void Update()
    {

        UpdateValue();
        RefreshData();

        foreach(var achievement in achievements)
        {
            if (!achievement.achieved && achievement.actualValue >= achievement.requiredValue)
            {
                achievement.achieved = true;
                display.DisplayAchievement(achievement.achievementName, achievement.description, achievement.image);
            }
        }

    }

    public void CheckCompletion(List<AchievementData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            //achievements[i].actualValue = data[i].actualValue;
            //achievements[i].achieved = data[i].achieved;

            if (data[i].achieved)
            {
                achievements[i].achieved = true;
            }
        }
    }

    public void RefreshData()
    {
        achievementsData = new List<AchievementData>();

        achievementsData.Clear();

        for (int i = 0; i < achievements.Count; i++)
        {
            achievementsData.Add(new AchievementData(achievements[i].achievementName, achievements[i].achieved, achievements[i].actualValue, achievements[i].requiredValue));
        }
    }

}

[Serializable]
public struct AchievementData
{
    public string title;
    public bool achieved;
    public int actualValue;
    public int requiredValue;
    public AchievementData(string title, bool achieved, int actual, int required)
    {
        this.title = title;
        this.achieved = achieved;
        this.actualValue = actual; 
        this.requiredValue = required;
    }
}
