using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControllerMain : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer vcBody;
    Transform playerPos;
    CameraFreezeAxis camFreeze;

    [Header("Settings")]
    [SerializeField] bool followPlayer;
    [SerializeField] float orthographicSize;
    [SerializeField] Vector2 damping;
    [SerializeField] Vector2 deadZone;

    [Header("Position")]
    [SerializeField] Vector3 targetOffset;
    [SerializeField] Vector2 cameraPos;

    [Header("Transition speed")]
    [SerializeField] float speed;
    [SerializeField] float zoomSpeed;

    [HideInInspector]
    public bool inMain;

    void Awake()
    {
        playerPos = FindObjectOfType<PlayerController>().transform;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        vcBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        camFreeze = virtualCamera.GetComponent<CameraFreezeAxis>();
        //MainSettings();
    }

    private void LateUpdate()
    {
        /*if (inMain)
        {

            float distance = Vector2.Distance(playerPos.position, virtualCamera.transform.position);

            if (followPlayer && distance < .15f)
            {
                virtualCamera.enabled = followPlayer;
            }
            else if (distance > .15f)
            {
                virtualCamera.transform.position = new Vector3(
                Mathf.MoveTowards(virtualCamera.transform.position.x, playerPos.position.x + targetOffset.x, speed * Time.deltaTime),
                Mathf.MoveTowards(virtualCamera.transform.position.y, playerPos.position.y + targetOffset.y, speed * Time.deltaTime),
                -10);
            }
            else if (!followPlayer)
            {
                virtualCamera.enabled = false;
                virtualCamera.transform.position = new Vector3(
                    Mathf.MoveTowards(virtualCamera.transform.position.x, cameraPos.x, speed * Time.deltaTime),
                    Mathf.MoveTowards(virtualCamera.transform.position.y, cameraPos.y, speed * Time.deltaTime),
                    -10);
            }

            virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(virtualCamera.m_Lens.OrthographicSize, orthographicSize, zoomSpeed * Time.deltaTime);
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, orthographicSize, zoomSpeed * Time.deltaTime);

            vcBody.m_TrackedObjectOffset.x = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.x, targetOffset.x, speed * Time.deltaTime);
            vcBody.m_TrackedObjectOffset.y = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.y, targetOffset.y, speed * Time.deltaTime);
        }*/
    }

    public void MainSettings()
    {
        inMain = true;

        camFreeze.axis = 0;

        vcBody.m_XDamping = damping.x;
        vcBody.m_YDamping = damping.y;
        vcBody.m_DeadZoneWidth = deadZone.x;
        vcBody.m_DeadZoneHeight = deadZone.y;
    }
}
