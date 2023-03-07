using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControllerMain : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer vcBody;
    Transform playerPos;

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

    Vector3 postPos;

    [HideInInspector]
    public bool inMain;

    void Awake()
    {
        playerPos = FindObjectOfType<PlayerController>().transform;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        vcBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        MainSettings();
    }

    private void LateUpdate()
    {
        if (inMain)
        {
            if (!followPlayer)
            {
                virtualCamera.enabled = false;
                virtualCamera.transform.position = new Vector3(
                    Mathf.MoveTowards(virtualCamera.transform.position.x, cameraPos.x, speed * Time.deltaTime),
                    Mathf.MoveTowards(virtualCamera.transform.position.y, cameraPos.y, speed * Time.deltaTime),
                    -10);
            }

            //Ca marche pas
            /*else if(postPos != playerPos.position)
            {
                virtualCamera.enabled = true;
                virtualCamera.transform.position = new Vector3(
                    Mathf.MoveTowards(postPos.x, playerPos.position.x, speed * Time.deltaTime), 
                    Mathf.MoveTowards(postPos.y, playerPos.position.y, speed * Time.deltaTime), 
                    -10);
            }

            virtualCamera.transform.position = postPos;*/

            virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(virtualCamera.m_Lens.OrthographicSize, orthographicSize, zoomSpeed * Time.deltaTime);
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, orthographicSize, zoomSpeed * Time.deltaTime);

            vcBody.m_TrackedObjectOffset.x = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.x, targetOffset.x, speed * Time.deltaTime);
            vcBody.m_TrackedObjectOffset.y = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.y, targetOffset.y, speed * Time.deltaTime);
        }
    }

    public void MainSettings()
    {
        inMain = true;

        postPos = virtualCamera.transform.position;

        virtualCamera.enabled = followPlayer;
        
        vcBody.m_XDamping = damping.x;
        vcBody.m_YDamping = damping.y;
        vcBody.m_DeadZoneWidth = deadZone.x;
        vcBody.m_DeadZoneHeight = deadZone.y;
    }
}
