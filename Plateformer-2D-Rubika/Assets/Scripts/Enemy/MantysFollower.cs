using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MantysFollower : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] float mantysSpeed;
    [SerializeField] float mantysFasterSpeed;
    [SerializeField] float mantysSprintSpeed;
    [SerializeField] Transform[] mantysTransform;
    [SerializeField] GameObject vCam;

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

    private void OnEnable()
    {
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
            if(!PlayerDeath.respawning)
                StartCoroutine(collision.GetComponent<PlayerDeath>().Respawn());
            StartCoroutine(RespawnEnemy());
        }
    }

    public IEnumerator RespawnEnemy()
    {
        respawning = true;
        yield return new WaitForSeconds(.1f);

        currentWaypoint = 0;
        transform.position = mantysTransform[currentWaypoint].position;
        actualSpeed = mantysSpeed;
        respawning = false;

        vCam.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
