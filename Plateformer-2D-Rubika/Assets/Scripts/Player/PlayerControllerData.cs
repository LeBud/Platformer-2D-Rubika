using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerControllerData : ScriptableObject
{
    [Header("Movement")]
    public float speed;
    public float accel;
    public float deccel;

    [Header("Jump")]
    public float jumpForce;
    public float coyoteTime = .1f;
    public float jumpBuffer = .1f;
    public float jumpCutForce = .1f;

    [Header("Glide")]
    public bool holdBtt;
    public bool canGlideJump;
    public float glideJumpForce;
    public float maxGlideTime;
    public float glideSpeedMult;
    public float curveMult;
    public AnimationCurve glideCurve;

    /*[Header("Fly Ability")]
    public float flyTimePerAphid;
    public float flyMaxTime;
    public float flySpeedMult;*/

    [Header("LadyBug Light")]
    public int maxAphid;
    public int maxTimeLightOn;

    [Header("Air Flow")]
    public float airFlowLerpSpeed = 2;

    [Header("Gravity")]
    public float normalGravitysScale = 5;
    public float airFlowGravity = 1;
    public float flyGravity = 0;

}
