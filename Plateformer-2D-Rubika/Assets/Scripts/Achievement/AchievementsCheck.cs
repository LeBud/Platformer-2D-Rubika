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

    bool jumpCountOneHundred, jumpCountOneThousand, deathOne, deathOneHundred, collectableOne, collectableHalf, collectableAll;

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
                new Achievement("Jump", "Jump 100 time.", tempSprite, jumpCountOneHundred),
                new Achievement("Jumping a lot !", "Jump 1000 time.", tempSprite, jumpCountOneThousand),
                new Achievement("First time ?", "Die for the 1st time.", tempSprite, deathOne),
                new Achievement("It's not that hard !", "Die 100 time.", tempSprite, deathOneHundred),
                //collectables Achievements
                new Achievement("Nice !", "Find your 1st collectables", tempSprite, collectableOne),
                new Achievement("Moooore !", "Find half of the collectables", tempSprite, collectableHalf),
                new Achievement("All of them !", "Find all the collectables", tempSprite, collectableAll),
                //gamplay Achievments
                new Achievement("Flying !", "Get in an aitflow for the 1st time.", tempSprite, airFlowUse),
                new Achievement("Bouncy", "Bounce on a jump pad fort the 1st time.", tempSprite, jumpPadUse),
                new Achievement("Too heavy !", "Bounce on a jump pad fort the 1st time.", tempSprite, breakPlatformUse),
                new Achievement("Lumios !", "Bounce on a jump pad fort the 1st time.", tempSprite, aphidUse),
                new Achievement("Watch out !", "Bounce on a jump pad fort the 1st time.", tempSprite, fallObjectUse),
                //story Achivements
                new Achievement("How did we get here ?", "Bounce on a jump pad fort the 1st time.", tempSprite, fallInCatacomb),
                new Achievement("Better luck next time !", "Bounce on a jump pad fort the 1st time.", tempSprite, surviveChase),
                new Achievement("Out of there !", "Bounce on a jump pad fort the 1st time.", tempSprite, exitCatacomb),
                new Achievement("Congratulations !", "Bounce on a jump pad fort the 1st time.", tempSprite, endGame)
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
        if (jumpCount > 99)
            jumpCountOneHundred = true;
        if(jumpCount > 999)
            jumpCountOneThousand= true;
        if (deathCount > 0)
            deathOne = true;
        if (deathCount > 99)
            deathOneHundred = true;
        if (collectableCount > 0)
            collectableOne = true;

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

    public Achievement(string title, string description, Sprite sprite, bool require) 
    {
        this.title = title;
        this.description = description;
        this.sprite = sprite;
        this.require = require;
    }

    public string title;
    public string description;
    public Sprite sprite;

    public AchievementsCheck achievementsCheck;

    //public Predicate<AchievementsCheck> requirement;

    public bool require;

    public bool achieved;

    public void UpdateCompletion()
    {
        if (achieved)
            return;

        if (require)
        {
            Debug.Log($"{title} : {description}");
            achieved = true;

            AchievementsCheck.RefreshData();

            saveSystem.SaveAchievements();
            display.DisplayAchievement(title, description, sprite);
        }
    }

    /*public bool RequirementMet()
    {
        return requirement(achievementsCheck) && !achieved;
    }*/
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