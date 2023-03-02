using UnityEngine;
using Cinemachine;


[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")]
public class CameraFreezeAxis : CinemachineExtension
{
    public bool freezeY;
    public bool freezeX;

    public float xPos;
    public float yPos;

    public float speed;

    Vector3 actualPos = new Vector3(0,0,-10);

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            if(freezeY)
            {
                actualPos = state.RawPosition;
                actualPos.x = xPos;
                state.RawPosition = actualPos;
            }

            if(freezeX)
            {
                actualPos = state.RawPosition;
                actualPos.y = yPos;
                state.RawPosition = actualPos;
            }
        }
    }

}
