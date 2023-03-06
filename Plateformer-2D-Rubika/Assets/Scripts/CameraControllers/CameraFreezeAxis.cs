using UnityEngine;
using Cinemachine;


[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")]
public class CameraFreezeAxis : CinemachineExtension
{
    public enum FreezeAxis {none, x, y}
    public FreezeAxis axis;

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
            if(axis == FreezeAxis.x)
            {
                actualPos = state.RawPosition;
                actualPos.x = xPos;
                state.RawPosition = actualPos;
            }

            if(axis == FreezeAxis.y)
            {
                actualPos = state.RawPosition;
                actualPos.y = yPos;
                state.RawPosition = actualPos;
            }
        }
    }

}
