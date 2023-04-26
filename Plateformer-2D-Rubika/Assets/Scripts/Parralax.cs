using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parralax : MonoBehaviour
{

    Transform camTrans;
    Vector3 lastCamPos;
    float textureUnitSize;

    [SerializeField] float parallaxEffectMult = .1f;

    private void Awake()
    {
        camTrans = Camera.main.transform;
        lastCamPos = camTrans.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = camTrans.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMult, deltaMovement.y, deltaMovement.z);

        lastCamPos = camTrans.position;

        if(Mathf.Abs(camTrans.position.x - transform.position.x) >= textureUnitSize)
        {
            float offsetPos = (camTrans.position.x - transform.position.x) % textureUnitSize;
            transform.position = new Vector3(camTrans.position.x + offsetPos, transform.position.y, transform.position.z);
        }
    }

}
