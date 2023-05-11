using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    [SerializeField]
    CinemachineVirtualCamera[] allVirtualCam;

    [Header("Controls lerping for Y damping during player jump/fall")]
    [SerializeField] float fallPanAmount = .25f;
    [SerializeField] float fallYPanTime = .35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    public bool isLerpingYDamping { get; private set; }
    public bool lerpedFromPlayerFalling { get; set; }

    Coroutine lerpYPanCoroutine;
    Coroutine panCameraCoroutine;

    CinemachineVirtualCamera currentCamera; 
    CinemachineFramingTransposer framingTransposer;

    float normYPanAmount;

    Vector2 startingTrackedOffset;

    private void Awake()
    {
        if(instance == null)
            instance = this;

        for(int i = 0; i < allVirtualCam.Length; i++)
        {
            if (allVirtualCam[i].enabled)
            {
                currentCamera = allVirtualCam[i];

                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        normYPanAmount = framingTransposer.m_YDamping;

        startingTrackedOffset = framingTransposer.m_TrackedObjectOffset;
    }

    #region Lerp Y
    public void LerpYDamping(bool playerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYACtion(playerFalling));
    }

    IEnumerator LerpYACtion(bool playerFalling)
    {
        isLerpingYDamping = true;

        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (playerFalling)
        {
            endDampAmount = fallPanAmount;
            lerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / fallYPanTime);
            framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        isLerpingYDamping = false;
    }
    #endregion

    #region PanCamera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                default:
                    break;

            }

            endPos *= panDistance;

            startPos = startingTrackedOffset;

            endPos += startPos;
        }
        else
        {
            startPos = framingTransposer.m_TrackedObjectOffset;
            endPos = startingTrackedOffset;
        }

        float elapsedTime = 0;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startPos, endPos, elapsedTime / panTime);
            framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }

    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        if(currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            cameraFromRight.enabled = true;

            cameraFromLeft.enabled = false;

            currentCamera = cameraFromRight;

            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else if (currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            cameraFromLeft.enabled = true;

            cameraFromRight.enabled = false;

            currentCamera = cameraFromLeft;

            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }


    }

    #endregion

    #region ResetCam

    public void ResetCam()
    {
        CinemachineVirtualCamera currentCam = allVirtualCam[0];

        for (int i = 0; i < allVirtualCam.Length; i++)
        {
            if (allVirtualCam[i].enabled)
            {
                currentCam = allVirtualCam[i];
            }
        }

        allVirtualCam[0].enabled = true;

        currentCam.enabled = false;

        currentCamera = allVirtualCam[0];

        framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    #endregion
}
