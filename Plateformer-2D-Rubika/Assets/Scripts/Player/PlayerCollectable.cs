using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectable : MonoBehaviour
{

    public static int collectable;


    public List<Collectable> collectablesList;

    public void CheckPointSave()
    {
        for (int i = 0; i < collectablesList.Count; i++)
        {
            collectablesList[i].checkPointSave = true;
        }

    }
}
