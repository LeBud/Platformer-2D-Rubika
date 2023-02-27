using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer vcBody;
    CameraControllerMain main;

    [Header("Settings")]
    [SerializeField] bool followPlayer;
    [SerializeField] int orthographicSize;
    [SerializeField] Vector2 damping;
    [SerializeField] Vector2 deadZone;

    [Header("Position")]
    [SerializeField] Vector3 targetOffset;
    [SerializeField] Vector2 cameraPos;

    [Header("Transition speed")]
    [SerializeField] float speed = 1;

    bool inTrigger;

    void Awake()
    {
        main = FindObjectOfType<CameraControllerMain>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        vcBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = true;
            main.inMain = false;

            if(!followPlayer)
            {
                virtualCamera.enabled = followPlayer;
            }

            vcBody.m_XDamping = damping.x;
            vcBody.m_YDamping = damping.y;
            vcBody.m_DeadZoneWidth = deadZone.x;
            vcBody.m_DeadZoneHeight = deadZone.y;
        }
    }

    private void LateUpdate()
    {
        if (inTrigger)
        {
            if (!followPlayer)
            {
                virtualCamera.transform.position = new Vector3(
                    Mathf.MoveTowards(virtualCamera.transform.position.x, cameraPos.x, speed * Time.deltaTime),
                    Mathf.MoveTowards(virtualCamera.transform.position.y, cameraPos.y, speed * Time.deltaTime), 
                    -10);
            }

            virtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(virtualCamera.m_Lens.OrthographicSize, orthographicSize, speed * Time.deltaTime);
            vcBody.m_TrackedObjectOffset.x = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.x, targetOffset.x, speed * Time.deltaTime);
            vcBody.m_TrackedObjectOffset.y = Mathf.MoveTowards(vcBody.m_TrackedObjectOffset.y, targetOffset.y, speed * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            main.MainSettings();
            inTrigger = false;
        }
    }

}
