using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisAI : MonoBehaviour
{
    [Header("PathFinding")]
    [SerializeField] Transform target;
    [SerializeField] float activateDistance;
    [SerializeField] float pathUpdateSeconds;

    [Header("Physics")]
    [SerializeField] float speed;
    [SerializeField] float nextWaypointDistance;
    [SerializeField] float jumpNodeHeightRequirement;
    [SerializeField] float jumpModifier;
    [SerializeField] float jumpCheckOffset;

    [Header("Custom Behavior")]
    [SerializeField] bool followEnable = true;
    [SerializeField] bool jumpEnable = true;
    [SerializeField] bool directionLook = true;
}
