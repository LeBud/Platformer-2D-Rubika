using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform playerTransform;
    [SerializeField] LadyBugLight ladyBugLight;
    [SerializeField] PlayerDeath deathNumber;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SaveData();
        if (Input.GetKeyUp(KeyCode.F9))
            LoadData();
    }

    void SaveData()
    {
        //Sauvegarder les données
        SavedData savedData = new SavedData
        {
            playerPositions = playerTransform.position,
            aphidNumber = ladyBugLight.aphidCount,
            deathNumber = deathNumber.deathCounter,
        };

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Sauvegarde effectué");
    }

    void LoadData()
    {
        //Récupérer les données sauvegardées
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        //Charger les données sauvergardées
        playerTransform.position = savedData.playerPositions;
        ladyBugLight.aphidCount = savedData.aphidNumber;
        deathNumber.deathCounter = savedData.deathNumber;

        Debug.Log("Chargement des données terminées");
    }
}

public class SavedData
{
    public Vector2 playerPositions;
    public int aphidNumber;
    public int deathNumber;
}
