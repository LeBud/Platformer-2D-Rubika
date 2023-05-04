using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsCheck : MonoBehaviour
{
    public static List<Achievement> achievements;
    public static List<AchievementData> achievementsData;
    public Sprite tempSprite;

    SaveSystem saveSystem;
    AchievementsDisplay display;

    [HideInInspector]
    public int jumpCount, deathCount, collectableCount;

    [HideInInspector]
    public bool airFlowUse, jumpPadUse, breakPlatformUse, aphidUse, fallObjectUse, fallInCatacomb, surviveChase, exitCatacomb, endGame;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
        display = FindObjectOfType<AchievementsDisplay>();

        InitializeAchievements();
    }

    private void InitializeAchievements()
    {
        if (achievements == null)
        {
            achievements = new List<Achievement>
            {
                //general Achievements
                new Achievement("Jump", "Jump 100 time.", tempSprite, (AchievementsCheck o) => jumpCount >= 100),
                new Achievement("Jumping a lot !", "Jump 1000 time.", tempSprite, (AchievementsCheck o) => jumpCount >= 1000),
                new Achievement("First time ?", "Die for the 1st time.", tempSprite, (AchievementsCheck o) => deathCount >= 1),
                new Achievement("It's not that hard !", "Die 100 time.", tempSprite, (AchievementsCheck o) => deathCount >= 100),
                //collectables Achievements
                new Achievement("Nice !", "Find your 1st collectables", tempSprite, (AchievementsCheck o) => collectableCount >= 1),
                new Achievement("Moooore !", "Find half of the collectables", tempSprite, (AchievementsCheck o) => collectableCount >= 5),
                new Achievement("All of them !", "Find all the collectables", tempSprite, (AchievementsCheck o) => collectableCount >= 10),
                //gamplay Achievments
                new Achievement("Flying !", "Get in an aitflow for the 1st time.", tempSprite, (AchievementsCheck o) => airFlowUse == true),
                new Achievement("Bouncy", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => jumpPadUse == true),
                new Achievement("Too heavy !", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => breakPlatformUse == true),
                new Achievement("Lumios !", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => aphidUse == true),
                new Achievement("Watch out !", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => fallObjectUse == true),
                //story Achivements
                new Achievement("How did we get here ?", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => fallInCatacomb == true),
                new Achievement("Better luck next time !", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => surviveChase == true),
                new Achievement("Out of there !", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => exitCatacomb == true),
                new Achievement("Congratulations !", "Bounce on a jump pad fort the 1st time.", tempSprite, (AchievementsCheck o) => endGame == true)
            };
        }

        foreach(Achievement achi in achievements)
        {
            achi.achievementsCheck = this;
            achi.saveSystem = saveSystem;
            achi.display = display;
        }


        RefreshData();
    }


    private void Update()
    {
        CheckAchievementCompletion();
    }

    private void CheckAchievementCompletion()
    {
        if (achievements.Count < 1)
            return;

        foreach (var achievement in achievements)
        {
            achievement.UpdateCompletion();
        }
    }

    public static void RefreshData()
    {
        achievementsData = new List<AchievementData>();

        achievementsData.Clear();
        
        for(int i = 0;i < achievements.Count;i++)
        {
            achievementsData.Add(new AchievementData(achievements[i].title, achievements[i].achieved));
            Debug.Log(achievements[i].achieved);
        }
    }

    public void CheckCompletion(List<AchievementData> data)
    {
        for(int i = 0; i < data.Count;i++)
        {
            if (data[i].achieved == true)
            {
                if (data[i].title == achievements[i].title)
                    achievements[i].achieved = true;
            }            
        }
    }

}

public class Achievement
{

    public SaveSystem saveSystem;
    public AchievementsDisplay display;

    public Achievement(string title, string description, Sprite sprite, Predicate<AchievementsCheck> requirement) 
    {
        this.title = title;
        this.description = description;
        this.sprite = sprite;
        this.requirement = requirement;
    }

    public string title;
    public string description;
    public Sprite sprite;

    public AchievementsCheck achievementsCheck;

    public Predicate<AchievementsCheck> requirement;

    public bool achieved;

    public void UpdateCompletion()
    {
        if (achieved)
            return;

        if (RequirementMet())
        {
            Debug.Log($"{title} : {description}");
            achieved = true;

            AchievementsCheck.RefreshData();

            saveSystem.SaveAchievements();
            display.DisplayAchievement(title, description, sprite);
        }
    }

    public bool RequirementMet()
    {
        return requirement(achievementsCheck) && !achieved;
    }
}

[Serializable]
public struct AchievementData
{
    public string title;
    public bool achieved;

    public AchievementData(string title, bool achieved)
    {
        this.title = title;
        this.achieved = achieved;
    }
}