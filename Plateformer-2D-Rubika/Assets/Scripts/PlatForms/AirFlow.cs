using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirFlow : MonoBehaviour
{

    [Header("Air Flow Settings")]
    public float airFlowForce;

    Quaternion angle;
    [HideInInspector]
    public Vector2 dir;

    private void Awake()
    {
        angle = Quaternion.AngleAxis(transform.rotation.z, Vector2.right);
        dir = angle * transform.right;
    }

}
