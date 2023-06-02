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
    [SerializeField] PlayerController playerController;
    NewAchievementSystem achievements;
    ParallaxSwap parallax;
    PauseMenu pauseMenu;
    private void Awake()
    {
        ladyBugLight = FindObjectOfType<LadyBugLight>();
        deathNumber= FindObjectOfType<PlayerDeath>();
        levelManager = FindObjectOfType<LevelManager>();
        achievements = FindObjectOfType<NewAchievementSystem>();
        gameManager = GetComponent<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
        playerTransform = ladyBugLight.transform;
        parallax = FindObjectOfType<ParallaxSwap>();
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    void GetComponents()
    {
        ladyBugLight = FindObjectOfType<LadyBugLight>();
        deathNumber = FindObjectOfType<PlayerDeath>();
        levelManager = FindObjectOfType<LevelManager>();
        achievements = FindObjectOfType<NewAchievementSystem>();
        gameManager = GetComponent<GameManager>();
        playerController = FindObjectOfType<PlayerController>();
        playerTransform = ladyBugLight.transform;
        parallax = FindObjectOfType<ParallaxSwap>();
    }

    private void Update()
    {
        if (pauseMenu.saveBtt.WasPressedThisFrame())
            SaveData();
        if (pauseMenu.loadBtt.WasPressedThisFrame())
            LoadData();
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
        savedData.achievementIntJump = achievements.jumpCount;

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
        
        if(achievements.achievements.Count > 0)
            savedData.achievementsData = NewAchievementSystem.achievementsData;

        achievements.CheckCompletion(savedData.achievementsData);

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

        savedData.collectablesList = gameManager.collectiblesSaveList;
        savedData.collectablesNum = gameManager.collectableCount;

        achievements.collectableCount = gameManager.collectableCount;

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
            aphidCharge = ladyBugLight.aphidCharge,
            deathNumber = deathNumber.deathCounter,
            checkPointNum = deathNumber.currentCheckPoint,
            currentRoomNum = deathNumber.checkPointRoom,
            collectablesList = gameManager.collectiblesSaveList,
            collectablesNum = gameManager.collectableCount,
            currentRoom = levelManager.currentRoom,
            lightCheckPoint = deathNumber.lightCheckPoint,
            canGlide = playerController.canGlide,
            parallaxNum = parallax.parallax,
            gardenAnim = playerController.gardenAnimator,
            blueAnim = playerController.blueAnimator,
            achievementIntJump = achievements.jumpCount,
        };

        if (achievements.achievements.Count > 0)
            savedData.achievementsData = NewAchievementSystem.achievementsData;

        achievements.collectableCount = gameManager.collectableCount;

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, jsonData);
        Debug.Log("Sauvegarde effectu�");
    }

    public void LoadData()
    {

        GetComponents();

        //R�cup�rer les donn�es sauvegard�es
        string filePath = Application.persistentDataPath + "/SaveFile.json";
        string jsonData = System.IO.File.ReadAllText(filePath);

        SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);

        //Charger les donn�es sauvergard�es
        playerTransform.position = savedData.playerPositions;
        ladyBugLight.aphidCount = savedData.aphidNumber;
        ladyBugLight.aphidCharge= savedData.aphidCharge;
        deathNumber.deathCounter = savedData.deathNumber;
        deathNumber.currentCheckPoint = savedData.checkPointNum;
        ActivatePreviousCheckPoints();
        deathNumber.checkPointRoom = savedData.currentRoomNum;
        gameManager.collectiblesSaveList = savedData.collectablesList;
        gameManager.collectableCount = savedData.collectablesNum;
        levelManager.startingRoom = savedData.currentRoomNum;
        levelManager.currentRoom = savedData.currentRoom;
        deathNumber.lightCheckPoint = savedData.lightCheckPoint;

        playerController.canGlide = savedData.canGlide;
        playerController.blueAnimator = savedData.blueAnim;
        playerController.gardenAnimator = savedData.gardenAnim;

        parallax.parallax = savedData.parallaxNum;

        gameManager.LoadCollectable();

        achievements.jumpCount = savedData.achievementIntJump;

        NewAchievementSystem.achievementsData = savedData.achievementsData;
        achievements.CheckCompletion(savedData.achievementsData);

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
    public bool canGlide;
    public int aphidNumber;
    public float aphidCharge;
    public int deathNumber;
    public int checkPointNum;
    public int currentRoomNum;
    public int collectablesNum;
    public List<Collected> collectablesList;
    public int currentRoom;
    public bool lightCheckPoint;
    public ParallaxSwap.Parallax parallaxNum;
    public bool gardenAnim, blueAnim;
    public List<AchievementData> achievementsData;
    public int achievementIntJump;
    public int achievementInt;
}
