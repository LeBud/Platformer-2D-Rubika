using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisCamFollow : MonoBehaviour
{

    [SerializeField] Transform player;

    private void LateUpdate()
    {
        if (MantysEnablerDisabler.mantisEnable)
            transform.position = new Vector2(transform.position.x, player.position.y);
    }
}
