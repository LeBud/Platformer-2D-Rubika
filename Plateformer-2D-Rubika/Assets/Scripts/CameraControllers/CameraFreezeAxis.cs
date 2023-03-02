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
            else if(freezeX)
            {
                actualPos = state.RawPosition;
                actualPos.y = yPos;
                state.RawPosition = actualPos;
            }
        }
    }

    private void LateUpdate()
    {
        /*if(freezeY)
            actualPos.x = Mathf.MoveTowards(actualPos.x, xPos, speed * Time.deltaTime);
        else
            actualPos.x = Mathf.MoveTowards(actualPos.x, transform.localPosition.x, speed * Time.deltaTime);


        if (freezeX)
            actualPos.y = Mathf.MoveTowards(actualPos.y, yPos, speed * Time.deltaTime);
        else
            actualPos.y = Mathf.MoveTowards(actualPos.y, transform.localPosition.y, speed * Time.deltaTime);*/

    }

}
