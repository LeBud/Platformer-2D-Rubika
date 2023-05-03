using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform playerTransform;
    [SerializeField] LadyBugLight ladyBugLight;
    [SerializeField] PlayerDeath deathNumber;
    [SerializeField] GameManager gameManager;
    [SerializeField] LevelManager levelManager;

    AchievementsCheck achievements;

    private void Awake()
    {
        ladyBugLight = FindObjectOfType<LadyBugLight>();
        deathNumber= FindObjectOfType<PlayerDeath>();
        levelManager = FindObjectOfType<LevelManager>();
        achievements = FindObjectOfType<AchievementsCheck>();
        gameManager = GetComponent<GameManager>();
        playerTransform = ladyBugLight.transform;

    }

    void GetComponents()
    {
        ladyBugLight = FindObjectOfType<LadyBugLight>();
        deathNumber = FindObjectOfType<PlayerDeath>();
        levelManager = FindObjectOfType<LevelManager>();
        achievements = FindObjectOfType<AchievementsCheck>();
        gameManager = GetComponent<GameManager>();
        playerTransform = ladyBugLight.transform;
    }

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

    public void SaveAchievements()
    {
        //Charger les donn�es
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        string jsonData = System.IO.File.ReadAllText(filePath);
        SavedData loadData = JsonUtility.FromJson<SavedData>(jsonData);

        //Sauvegarder les donn�es
        SavedData savedData = loadData;
        
        if(AchievementsCheck.achievements.Count > 0)
            savedData.achievementsList = AchievementsCheck.achievementsData;

        achievements.CheckCompletion(savedData.achievementsList);

        //Sauvegarder les donn�es
        jsonData = JsonUtility.ToJson(savedData);
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public void SaveCollectable()
    {
        //Charger les donn�es
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        string jsonData = System.IO.File.ReadAllText(filePath);
        SavedData loadData = JsonUtility.FromJson<SavedData>(jsonData);

        //Sauvegarder les donn�es
        SavedData savedData = loadData;

        savedData.collectablesList = gameManager.collectables;
        savedData.collectablesNum = gameManager.collectableNum;

        achievements.collectableCount = gameManager.collectableNum;

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
            currentRoom = levelManager.currentRoom,
            lightCheckPoint = deathNumber.lightCheckPoint,
        };

        if (AchievementsCheck.achievements.Count > 0)
            savedData.achievementsList = AchievementsCheck.achievementsData;

        achievements.collectableCount = gameManager.collectableNum;

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Sauvegarde effectu�");
    }

    public void LoadData()
    {

        GetComponents();
        gameManager.GetCollectables();

        //R�cup�rer les donn�es sauvegard�es
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        //Charger les donn�es sauvergard�es
        playerTransform.position = savedData.playerPositions;
        ladyBugLight.aphidCount = savedData.aphidNumber;
        deathNumber.deathCounter = savedData.deathNumber;
        deathNumber.currentCheckPoint = savedData.checkPointNum;
        ActivatePreviousCheckPoints();
        deathNumber.checkPointRoom = savedData.currentRoomNum;
        gameManager.collectables = savedData.collectablesList;
        gameManager.collectableNum = savedData.collectablesNum;
        levelManager.startingRoom = savedData.currentRoomNum;
        levelManager.currentRoom = savedData.currentRoom;
        deathNumber.lightCheckPoint = savedData.lightCheckPoint;

        gameManager.LoadCollectable();

        achievements.CheckCompletion(savedData.achievementsList);

        Debug.Log("Chargement des donn�es termin�es");
    }

    void ActivatePreviousCheckPoints()
    {
        CheckPoint[] check = FindObjectsOfType<CheckPoint>();
        for(int i = 0; i < check.Length; i++)
        {
            if (check[i].checkPointNum <= deathNumber.currentCheckPoint)
                check[i].Active();
        }
    }


    public void EreaseSave()
    {
        string filePath = Application.persistentDataPath + "/SaveFile.json";

        System.IO.File.Delete(filePath);
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
    public int currentRoom;
    public bool lightCheckPoint;
    public List<AchievementData> achievementsList;
}
