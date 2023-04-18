using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int collectableNum;

    public List<Collected> collectables = new List<Collected>();

    private void Awake()
    {
        if (MainMenu.ereaseSave) FindObjectOfType<SaveSystem>().SaveData();
        else if (MainMenu.loadSave) FindObjectOfType<SaveSystem>().LoadData();

        Collectable[] collectable = FindObjectsOfType<Collectable>();

        for (int i = 0; i < collectable.Length; i++)
        {
            collectables.Add(new Collected { collectable = collectable[i], taken = collectable[i].taken });
            collectable[i].num = i;
        }

            LoadCollectable();
    }

    public void LoadCollectable()
    {
        for(int i = 0; i < collectables.Count; i++)
        {

            if (collectables[i].taken)
                collectables[i].collectable.gameObject.SetActive(false);
            else
                collectables[i].collectable.gameObject.SetActive(true);
        }
    }
}

[System.Serializable]
public class Collected
{
    public Collectable collectable;
    public bool taken;
}
