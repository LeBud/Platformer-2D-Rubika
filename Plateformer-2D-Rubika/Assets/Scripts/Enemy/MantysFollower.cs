using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class MantysFollower : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sprite;

    [SerializeField] CinemachineFramingTransposer vBody;

    [SerializeField] CameraFreezeAxis camFreeze;

    [Header("Settings")]
    [SerializeField] float mantysSpeed;
    [SerializeField] float mantysMaxSpeed;
    [SerializeField] float mantysMinSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] public Transform[] respawnPos;
    [SerializeField] public Transform[] jumpPoints;
    [SerializeField] GameObject vCam;

    float actualSpeed;

    public int jumpPointNum;

    [HideInInspector]
    public int currentWaypoint;
    [HideInInspector]
    public int respawnCheckPoint;
    [HideInInspector]
    public int jumpPointCheckPoint;

    bool respawning;

    GameObject playerController;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite= GetComponent<SpriteRenderer>();
        playerController = GameObject.FindGameObjectWithTag("Player");
        actualSpeed = mantysSpeed;
        currentWaypoint = 0;
    }

    private void OnEnable()
    {
        playerController = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (PauseMenu.gameIsPause) return;
        
        Flip();

        if (!MantysEnablerDisabler.mantisEnable)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        NewMovementSystem();
        ChangeSpeed();

    }

    void NewMovementSystem()
    {
        if (respawning) return;

        if (jumpPointNum < jumpPoints.Length)
        {
            float distance = Vector2.Distance(transform.position, jumpPoints[jumpPointNum].position);

            //Debug.Log(distance);

            if (distance < 3f)
            {
                float force = jumpForce;
                if (rb.velocity.y < 0)
                    force -= rb.velocity.y;

                rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

                jumpPointNum++;
            }
        }

        Vector2 dir = transform.position - playerController.transform.position;

        float moveForce;

        if(dir.normalized.x < 0)
            moveForce = actualSpeed;
        else
            moveForce = -actualSpeed;

        rb.velocity = new Vector2(moveForce, rb.velocity.y);


    }

    void Flip()
    {
        camFreeze.targetY = playerController.transform.position.y;

        Vector2 dir = transform.position - playerController.transform.position;

        if(dir.normalized.x < 0)
        {
            sprite.flipX = false;
            vBody.m_TrackedObjectOffset = new Vector3(15.5f, 2, 0);
        }
        else
        {
            sprite.flipX = true;
            vBody.m_TrackedObjectOffset = new Vector3(-15.5f, 2, 0);
        }
    }

    /*void WaypointsMovement()
    {
        if (currentWaypoint < respawnPos.Length)
        {

            float distance = Vector2.Distance(transform.position, respawnPos[currentWaypoint].position);

            if (distance > .1f)
            {
                transform.position = new Vector3(
                    Mathf.MoveTowards(transform.position.x, respawnPos[currentWaypoint].position.x, actualSpeed * Time.deltaTime),
                    Mathf.MoveTowards(transform.position.y, respawnPos[currentWaypoint].position.y, actualSpeed * Time.deltaTime),
                    0);
                Debug.Log(1);
            }
            else
            {
                if (!MantysEnablerDisabler.mantisEnable) return;
                currentWaypoint++;
            }
        }

    }*/

    void ChangeSpeed()
    {

        float distance = Vector2.Distance(transform.position, playerController.transform.position);

        if (distance < 14)
            mantysSpeed = mantysMinSpeed;
        else if(distance > 18)
            mantysSpeed = mantysMaxSpeed;
        else
            mantysSpeed = (mantysMinSpeed + mantysMaxSpeed) / 2f ;

        actualSpeed = Mathf.MoveTowards(actualSpeed, mantysSpeed, 40f * Time.deltaTime);

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

        transform.position = respawnPos[respawnCheckPoint].position;

        jumpPointNum = jumpPointCheckPoint;

        actualSpeed = mantysMinSpeed;

        respawning = false;

        vCam.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
