using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform playerTransform;
    [SerializeField] LadyBugLight ladyBugLight;
    [SerializeField] PlayerDeath deathNumber;
    [SerializeField] GameManager gameManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SaveData();
        if (Input.GetKeyUp(KeyCode.F9))
            LoadData();
        if (Input.GetKeyUp(KeyCode.F6))
            SaveDeath();
    }


    public void SaveDeath()
    {
        //Charger les donn�es
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        string jsonData = System.IO.File.ReadAllText(filePath);
        SavedData loadData = JsonUtility.FromJson<SavedData>(jsonData);

        //Sauvegarder les donn�es
        SavedData savedData = loadData;
        savedData.deathNumber = deathNumber.deathCounter;

        //Sauvegarder les donn�es
        jsonData = JsonUtility.ToJson(savedData);
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public void SaveData()
    {
        //Sauvegarder les donn�es
        SavedData savedData = new SavedData
        {
            playerPositions = playerTransform.position,
            aphidNumber = ladyBugLight.aphidCount,
            deathNumber = deathNumber.deathCounter,
            checkPointNum = deathNumber.currentCheckPoint,
            currentRoomNum = deathNumber.checkPointRoom,
            collectablesList = gameManager.collectables,
            collectablesNum = gameManager.collectableNum,
        };

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Sauvegarde effectu�");
    }

    public void LoadData()
    {
        //R�cup�rer les donn�es sauvegard�es
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        //Charger les donn�es sauvergard�es
        playerTransform.position = savedData.playerPositions;
        ladyBugLight.aphidCount = savedData.aphidNumber;
        deathNumber.deathCounter = savedData.deathNumber;
        deathNumber.currentCheckPoint = savedData.checkPointNum;
        deathNumber.checkPointRoom = savedData.currentRoomNum;
        gameManager.collectables = savedData.collectablesList;
        gameManager.collectableNum = savedData.collectablesNum;

        gameManager.LoadCollectable();

        Debug.Log("Chargement des donn�es termin�es");
    }
}

public class SavedData
{
    public Vector2 playerPositions;
    public int aphidNumber;
    public int deathNumber;
    public int checkPointNum;
    public int currentRoomNum;
    public int collectablesNum;
    public List<Collected> collectablesList;
}
