using UnityEngine;
using Cinemachine;


[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")]
public class CameraFreezeAxis : CinemachineExtension
{
    public bool followXOnly;
    public bool followYOnly;

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
            if(followXOnly)
            {
                var pos = state.RawPosition;
                actualPos.x = pos.x;
                state.RawPosition = actualPos;
            }
            else if(followYOnly)
            {
                var pos = state.RawPosition;
                actualPos.y = pos.y;
                state.RawPosition = actualPos;
            }
        }
    }

    private void LateUpdate()
    {
        if (followXOnly)
            actualPos.x = Mathf.MoveTowards(actualPos.x, xPos, speed * Time.deltaTime);
        
        if (followYOnly)
            actualPos.y = Mathf.MoveTowards(actualPos.y, yPos, speed * Time.deltaTime);
        
    }

}
