using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MantysFollower : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] float mantysSpeed;
    [SerializeField] float mantysMaxSpeed;
    [SerializeField] float mantysMinSpeed;
    [SerializeField] public Transform[] mantysTransform;
    [SerializeField] GameObject vCam;

    float actualSpeed;

    //[HideInInspector]
    public int currentWaypoint;

    bool respawning;

    GameObject playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
        actualSpeed = mantysSpeed;
        currentWaypoint = 0;
    }


    private void Update()
    {
        if (PauseMenu.gameIsPause) return;

        if (respawning) return;


        if(currentWaypoint < mantysTransform.Length)
        {

            float distance = Vector2.Distance(transform.position, mantysTransform[currentWaypoint].position);
            
            if(distance > .1f)
            {
                transform.position = new Vector3(
                    Mathf.MoveTowards(transform.position.x, mantysTransform[currentWaypoint].position.x, actualSpeed * Time.deltaTime),
                    Mathf.MoveTowards(transform.position.y, mantysTransform[currentWaypoint].position.y, actualSpeed * Time.deltaTime),
                    0);
                Debug.Log(1);
            }
            else
            {
                if (!MantysEnablerDisabler.mantisEnable) return;
                currentWaypoint++;
            }
        }

        actualSpeed = Mathf.MoveTowards(actualSpeed, mantysSpeed, 2f * Time.deltaTime);

        ChangeSpeed();
    }

    void ChangeSpeed()
    {

        if (respawning) return;

        float distance = Vector2.Distance(transform.position, playerController.transform.position);

        

        if (distance < 12)
            mantysSpeed = mantysMinSpeed;
        else if(distance > 18)
            mantysSpeed = mantysMaxSpeed;
        else
            mantysSpeed = (mantysMinSpeed + mantysMaxSpeed) / 2.4f ;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("respawn");

        if (collision.CompareTag("Player"))
        {
            if(!PlayerDeath.respawning)
                StartCoroutine(collision.GetComponent<PlayerDeath>().Respawn());
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
