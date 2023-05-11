using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisCamFollow : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] Transform mantis;

    private void LateUpdate()
    {
        if (MantysEnablerDisabler.mantisEnable)
            transform.position = new Vector2(mantis.position.x, player.position.y);
    }
}
