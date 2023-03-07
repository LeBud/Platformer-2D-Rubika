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
                //Récupère la position de la camera
                actualPos = state.RawPosition;
                //On bloque la position X à la position voulue
                actualPos.x = targetX;
                //Dit à la caméra de ce mettre à la position actuelle en Z et Y et de rester sur la même position X
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
