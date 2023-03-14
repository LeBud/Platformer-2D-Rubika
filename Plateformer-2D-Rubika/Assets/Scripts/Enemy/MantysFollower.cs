using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantysFollower : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] float followSpeed;

    PlayerController playerController;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        Vector3 dir = playerController.transform.position - transform.position;

        transform.Translate(dir * followSpeed * Time.deltaTime);
    }


}
