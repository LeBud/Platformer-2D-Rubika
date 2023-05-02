using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform playerTransform;
    [SerializeField] float flipYRotationTime = .5f;

    SpriteFlip flip;

    Coroutine turnCoroutine;

    bool facingRight;

    private void Awake()
    {
        flip = FindObjectOfType<SpriteFlip>();

        facingRight = flip.facingRight;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    IEnumerator FlipYLerp()
    {
        float startRot = transform.localEulerAngles.y;
        float endRot = DetermineRot();
        float yRot = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRot = Mathf.Lerp(startRot, endRot, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRot, 0f);

            yield return null;
        }
    }

    float DetermineRot()
    {
        facingRight = !facingRight;

        if (facingRight)
            return 0f;
        else
            return 180f;
    }
}
