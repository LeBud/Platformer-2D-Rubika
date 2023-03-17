using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] List<GameObject> levels;
    [SerializeField] int startingRoom;

    private void Start()
    {
        
    }


    private void Update()
    {
        
        ManageLevels();

    }


    void ManageLevels()
    {

    }
}
