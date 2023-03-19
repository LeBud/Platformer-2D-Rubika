using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantysFollower : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] float mantysSpeed;
    [SerializeField] float mantysFasterSpeed;
    [SerializeField] float mantysSprintSpeed;
    [SerializeField] Transform[] mantysTransform;

    float actualSpeed;
    float newSpeed;

    int currentWaypoint;

    bool respawning;

    private void Start()
    {
        actualSpeed = mantysSpeed;
        currentWaypoint = 0;
        StartCoroutine(ChangeSpeed());
    }

    private void Update()
    {
        if (PauseMenu.gameIsPause) return;

        if (respawning) return;

        if(currentWaypoint < mantysTransform.Length)
        {
            if(transform.position != mantysTransform[currentWaypoint].position)
            {
                transform.position = new Vector3(
                    Mathf.MoveTowards(transform.position.x, mantysTransform[currentWaypoint].position.x, actualSpeed * Time.deltaTime),
                    Mathf.MoveTowards(transform.position.y, mantysTransform[currentWaypoint].position.y, actualSpeed * Time.deltaTime),
                    0);
            }
            else
                currentWaypoint++;
        }

        actualSpeed = Mathf.MoveTowards(actualSpeed, newSpeed, 20f * Time.deltaTime);

    }

    IEnumerator ChangeSpeed()
    {

        if (respawning) yield break;
        
        newSpeed = mantysSpeed;

        yield return new WaitForSeconds(1);

        newSpeed = mantysFasterSpeed;

        yield return new WaitForSeconds(1);

        StartCoroutine(ChangeSpeed());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("resapwn");

        if (collision.CompareTag("Player"))
        {
            StartCoroutine(collision.GetComponent<PlayerController>().Respawn());
            StartCoroutine(RespawnEnemy());
        }
    }

    IEnumerator RespawnEnemy()
    {
        respawning = true;
        yield return new WaitForSeconds(1);

        currentWaypoint = 0;
        transform.position = mantysTransform[currentWaypoint].position;
        actualSpeed = mantysSpeed;

        yield return new WaitForSeconds(2);
        respawning = false;

        StartCoroutine(ChangeSpeed());
    }

}
