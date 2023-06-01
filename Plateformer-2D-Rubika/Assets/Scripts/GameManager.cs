using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int collectableCount;

    public List<Collectable> inGameCollectibles = new List<Collectable>();
    [HideInInspector]
    public List<Collected> collectiblesSaveList = new List<Collected>();

    SaveSystem saveSystem;
    NewAchievementSystem achievementSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();
        achievementSystem = GetComponent<NewAchievementSystem>();

        if (MainMenu.ereaseSave)
        {
            saveSystem.EreaseSave();
            FindObjectOfType<PlayerController>().canGlide = false;
            FindObjectOfType<PlayerController>().gardenAnimator = true;
            FindObjectOfType<ParallaxSwap>().parallax = ParallaxSwap.Parallax.gardenParallax;
            foreach(var achievement in achievementSystem.achievements)
            {
                achievement.actualValue = 0;
                achievement.achieved = false;
            }
        }
        else if (MainMenu.loadSave) FindObjectOfType<SaveSystem>().LoadData();

        
        if(MainMenu.ereaseSave)
            RefreshList();

        LoadCollectable();

    }

    private void Start()
    {
        if (MainMenu.ereaseSave) saveSystem.SaveData();
    }

    public void LoadCollectable()
    {
        for(int i = 0; i < collectiblesSaveList.Count; i++)
        {
            if (collectiblesSaveList[i].taken)
                inGameCollectibles[i].gameObject.SetActive(false);
            else
                inGameCollectibles[i].gameObject.SetActive(true);
        }
    }

    public void RefreshList()
    {
        collectiblesSaveList.Clear();

        for (int i = 0; i < inGameCollectibles.Count; i++)
        {
            Debug.Log(i);
            collectiblesSaveList.Add(new Collected { ID = inGameCollectibles[i].ID, taken = inGameCollectibles[i].taken });
        }

    }

}

[Serializable]
public struct Collected
{
    public int ID;
    public bool taken;
}
