using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    PlayerControllerData controllerData;

    CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer vcBody;

    Rigidbody2D rb;

    float centerCamTimer;
    float inputYAxis;

    void Awake()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        vcBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        rb = GetComponent<Rigidbody2D>();
        controllerData = GetComponent<PlayerController>().playerControllerData;
    }

    void LateUpdate()
    {

        inputYAxis = Input.GetAxisRaw("Vertical");
        if (rb.velocity.x < 5 && rb.velocity.x > -5)
            centerCamTimer += Time.deltaTime;
        else
            centerCamTimer = 0;

        //decenter Y Axis
        if(inputYAxis > .75f)
            vcBody.m_TrackedObjectOffset.y = controllerData.camOffsetY;
        else if(inputYAxis < -.75f)
        vcBody.m_TrackedObjectOffset.y = -controllerData.camOffsetY;
        else
            vcBody.m_TrackedObjectOffset.y = 0;
        
        //decenter X Axis
        if(rb.velocity.x > 5)
            vcBody.m_TrackedObjectOffset.x = controllerData.camOffsetX;
        else if (rb.velocity.x < -5)
            vcBody.m_TrackedObjectOffset.x = -controllerData.camOffsetX;
        else
        {
            if(controllerData.timeToRecenter < centerCamTimer)
                vcBody.m_TrackedObjectOffset.x = 0;
        }

    }
}
