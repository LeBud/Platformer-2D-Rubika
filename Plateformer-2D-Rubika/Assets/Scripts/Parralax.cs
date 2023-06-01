using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{

    Transform camTrans;
    Vector3 lastCamPos;
    float textureUnitSize;
    float textureUnitSizeY;

    [SerializeField] float parallaxEffectMult = .1f;
    [SerializeField] float parallaxEffectMultY = .1f;

    private void Awake()
    {
        camTrans = Camera.main.transform;
        lastCamPos = camTrans.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = camTrans.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMult, deltaMovement.y * parallaxEffectMultY, deltaMovement.z);

        lastCamPos = camTrans.position;

        float offsetPosX;
        float offsetPosY;

        if (Mathf.Abs(camTrans.position.x - transform.position.x) >= textureUnitSize)
        {
            offsetPosX = (camTrans.position.x - transform.position.x) % textureUnitSize;
            transform.position = new Vector3(camTrans.position.x + offsetPosX, transform.position.y, transform.position.z);
        }
        /*if (Mathf.Abs(camTrans.position.y - transform.position.y) >= textureUnitSizeY / 3)
        {
            offsetPosY = (camTrans.position.y - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3(transform.position.x, camTrans.position.y + offsetPosY, transform.position.z);
        }*/

    }

}
