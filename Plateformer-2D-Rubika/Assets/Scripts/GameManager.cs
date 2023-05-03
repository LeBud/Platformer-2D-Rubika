using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int collectableNum;

    public List<Collectable> collectableList = new List<Collectable>();
    public List<Collected> collectables = new List<Collected>();

    SaveSystem saveSystem;

    private void Awake()
    {
        saveSystem = GetComponent<SaveSystem>();

        if (MainMenu.ereaseSave) saveSystem.EreaseSave();
        else if (MainMenu.loadSave) FindObjectOfType<SaveSystem>().LoadData();

        Collectable[] col = FindObjectsOfType<Collectable>();

        foreach(Collectable coll in col)
        {
            collectableList.Add(coll);
        }

        if(MainMenu.ereaseSave)
            RefreshList();

        LoadCollectable();
    }

    private void Start()
    {
        if (MainMenu.ereaseSave) saveSystem.SaveData();
    }

    public void GetCollectables()
    {
        collectableList.Clear();

        Collectable[] col = FindObjectsOfType<Collectable>();

        foreach (Collectable coll in col)
        {
            collectableList.Add(coll);
        }
    }

    public void LoadCollectable()
    {
        /*for(int i = 0; i < collectables.Count; i++)
        {
            if (collectables[i].taken)
                collectableList[i].gameObject.SetActive(false);
            else
                collectableList[i].gameObject.SetActive(true);
        }*/
    }

    public void RefreshList()
    {
        collectables.Clear();

        for (int i = 0; i < collectableList.Count; i++)
        {
            collectables.Add(new Collected { ID = i, taken = collectableList[i].taken });
            collectableList[i].num = i;
        }

    }

}

[Serializable]
public struct Collected
{
    public int ID;
    public bool taken;
}
