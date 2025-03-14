using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] List<GameObject> levels;
    public int startingRoom;
    public int currentRoom;

    public static bool respawning;

    private void Awake()
    {
        currentRoom = startingRoom;
        
        for(int i = 0; i < levels.Count; i++)
        {
            levels[i].GetComponent<LevelRoom>().roomNum = i;
            levels[i].SetActive(false);
        }

        for (int i = -1; i < 2; i++)
        {
            if (currentRoom + i >= 0 && currentRoom + i < levels.Count)
            {
                levels[currentRoom + i].SetActive(true);
            }
        }

    }


    private void Update()
    {
        if (respawning) return;
        ManageLevels();

    }


    void ManageLevels()
    {

        for (int i = -1; i < 2; i++)
        {
            if ((currentRoom + i) >= 0 && (currentRoom + i) < levels.Count)
            {
                levels[currentRoom + i].SetActive(true);
            }
        }

        for(int i = 0; i < levels.Count;i++)
        {
            if(i < (currentRoom - 1) || i > (currentRoom + 1))
                levels[i].SetActive(false);
        }
    }
  
    public void ActiveAllLevel()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].SetActive(true);
        }

    }

}
