using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AchievementsSO : ScriptableObject
{

    [Header("Achievement Settings")]
    public string achievementName;
    public string description;
    public Sprite image;
    //Pour les bool utiliser un system de 1 = true et 0 = false
    public int actualValue;
    public int requiredValue;

    public bool achieved = false;
}
