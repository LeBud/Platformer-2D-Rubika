using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisAI : MonoBehaviour
{
    Transform playerController;

    [Header("WayPoint")]
    public Transform[] waypoints;
    public int currentWP;
    public int respawnWP;

    [Header("Parameters")]
    [SerializeField] float minSpeed;
    [SerializeField] float midSpeed;
    [SerializeField] float maxSpeed;
    float currentSpeed;
    float targetSpeed;

    AudioSource source;
    [Header("Sounds")]
    public AudioClip walkSound;
    public AudioClip screamSound;

    bool screamSoundPlayong;

    public static bool respawning;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").transform;
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (PauseMenu.gameIsPause) return;
        if (!MantysEnablerDisabler.mantisEnable) return;

        WaypointsMovement();
        ChangeSpeed();

        if (!screamSoundPlayong)
            StartCoroutine(ScreamSound());
    }

    public void PawSound()
    {
        source.PlayOneShot(walkSound);
    }

    IEnumerator ScreamSound()
    {
        screamSoundPlayong = true;
        int time = Random.Range(5, 20);

        source.PlayOneShot(screamSound);

        yield return new WaitForSeconds(time);
        screamSoundPlayong = false;
    }

    void WaypointsMovement()
    {
        if (currentWP < waypoints.Length)
        {
            float distance = Vector2.Distance(transform.position, waypoints[currentWP].position);

            NextWaypoint(currentWP, distance);

            if (distance > .01f)
            {
                transform.position = new Vector3(
                    Mathf.MoveTowards(transform.position.x, waypoints[currentWP].position.x, currentSpeed * Time.deltaTime),
                    Mathf.MoveTowards(transform.position.y, waypoints[currentWP].position.y, currentSpeed * Time.deltaTime),
                    0);
            }
            else
            {
                if (!MantysEnablerDisabler.mantisEnable) return;
                currentWP++;
            }
        }
    }

    void NextWaypoint(int actualWP, float distanceWP)
    {
        if(distanceWP < .05f && actualWP < waypoints.Length)
        {
            currentWP = actualWP + 1;
        }
    }

    void ChangeSpeed()
    {

        float distance = Vector2.Distance(transform.position, playerController.transform.position);

        if (distance < 8)
            targetSpeed = 10;
        else if (distance > 15)
            targetSpeed = 20;
        else
            targetSpeed = 14;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, 40f * Time.deltaTime);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!PlayerDeath.respawning)
                StartCoroutine(collision.GetComponent<PlayerDeath>().Respawn());
        }
    }

    public IEnumerator Respawn()
    {
        Debug.Log("die");

        respawning = true;

        yield return new WaitForSeconds(.1f);

        transform.position = waypoints[respawnWP].position;

        currentSpeed = minSpeed;

        respawning = false;

        gameObject.SetActive(false);
    }


}
