using UnityEngine;
using Cinemachine;


[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")]
public class CameraFreezeAxis : CinemachineExtension
{
    public enum FreezeAxis {none, x, y}
    public FreezeAxis axis;

    public float targetX;
    public float targetY;

    Vector3 actualPos = new Vector3(0,0,-10);

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            if(axis == FreezeAxis.x)
            {
                //R�cup�re la position de la camera
                actualPos = state.RawPosition;
                //On bloque la position X � la position voulue
                actualPos.x = targetX;
                //Dit � la cam�ra de ce mettre � la position actuelle en Z et Y et de rester sur la m�me position X
                state.RawPosition = actualPos;
            }

            if(axis == FreezeAxis.y)
            {
                actualPos = state.RawPosition;
                actualPos.y = targetY;
                state.RawPosition = actualPos;
            }
        }
    }

}
