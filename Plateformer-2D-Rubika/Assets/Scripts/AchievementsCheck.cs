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
        saveSystem = FindObjectOfType<SaveSystem>();
        display = FindObjectOfType<AchievementsDisplay>();

        InitializeAchievements();
    }

    public bool AchievementUnlocked(string achievementName)
    {
        bool result = false;

        if (achievements == null)
            return false;

        Achievement[] achievementsArray = achievements.ToArray();
        Achievement a = Array.Find(achievementsArray, ach => achievementName == ach.title);

        if (a == null)
            return false;

        result = a.achieved;

        return result;
    }

    private void InitializeAchievements()
    {
        if (achievements == null)
        {
            achievements = new List<Achievement>();
            //general Achievements
            achievements.Add(new Achievement("Jump", "Jump 100 time.", tempSprite, (object o) => jumpCount >= 100));
            achievements.Add(new Achievement("Jumping a lot !", "Jump 1000 time.", tempSprite, (object o) => jumpCount >= 1000));
            achievements.Add(new Achievement("First time ?", "Die for the 1st time.", tempSprite, (object o) => deathCount >= 1));
            achievements.Add(new Achievement("It's not that hard !", "Die 100 time.", tempSprite, (object o) => deathCount >= 100));
            //collectables Achievements
            achievements.Add(new Achievement("Nice !", "Find your 1st collectables", tempSprite, (object o) => collectableCount >= 1));
            //gamplay Achievments
            achievements.Add(new Achievement("Flying !", "Get in an aitflow for the 1st time.", tempSprite, (object o) => airFlowUse == true));
            achievements.Add(new Achievement("Bouncy", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => jumpPadUse == true));
            achievements.Add(new Achievement("Too heavy !", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => breakPlatformUse == true));
            achievements.Add(new Achievement("Lumios !", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => aphidUse == true));
            achievements.Add(new Achievement("Watch out !", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => fallObjectUse == true));
            //story Achivements
            achievements.Add(new Achievement("How did we get here ?", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => fallInCatacomb == true));
            achievements.Add(new Achievement("Better luck next time !", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => surviveChase == true));
            achievements.Add(new Achievement("Out of there !", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => exitCatacomb == true));
            achievements.Add(new Achievement("Congratulations !", "Bounce on a jump pad fort the 1st time.", tempSprite, (object o) => endGame == true));
        }

        foreach(Achievement achi in achievements)
        {
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
        if (achievements == null)
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

    public Achievement(string title, string description, Sprite sprite, Predicate<object> requirement) 
    {
        this.title = title;
        this.description = description;
        this.sprite = sprite;
        this.requirement = requirement;
    }

    public string title;
    public string description;
    public Sprite sprite;


    public Predicate<object> requirement;

    public bool achieved;

    public void UpdateCompletion()
    {
        if (achieved) return;

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
        return requirement.Invoke(null);
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
