using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantysFollower : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] float mantysSpeed;
    [SerializeField] Transform[] mantysTransform;

    int currentWaypoint;

    private void Start()
    {
        currentWaypoint = 0;
    }

    private void Update()
    {
        if(currentWaypoint < mantysTransform.Length)
        {
            if(transform.position != mantysTransform[currentWaypoint].position)
            {
                transform.position = new Vector3(
                    Mathf.MoveTowards(transform.position.x, mantysTransform[currentWaypoint].position.x, mantysSpeed * Time.deltaTime),
                    Mathf.MoveTowards(transform.position.y, mantysTransform[currentWaypoint].position.y, mantysSpeed * Time.deltaTime),
                    0);
            }
            else
                currentWaypoint++;
        }
    }

}
