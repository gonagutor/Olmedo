using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCScript : MonoBehaviour
{
    public float maximumRadius = 5f;
    public float waitTime = .5f;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public float desistTime = 20f;
    public float fallRecoveryTime = 1f;
    public LayerMask shouldFallWhenIn;

    [Header("Impostor")]
    public bool isImpostor = false;
    public int fakeFallProbability = 10;

    [Header("Audios")]
    public AudioClip[] audioClips;

    [Header("Debug")]
    public bool showDebugMessages = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private AudioSource audioSource;

    private Vector2 destination;
    private Path path;
    private int currentWayPoint = 0;
    private float desistCounter = 0;
    private bool hasFinished = false;
    private bool hasFallen = false;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        seeker = this.GetComponent<Seeker>();
        rb = this.GetComponent<Rigidbody2D>();
        audioSource = this.GetComponent<AudioSource>();

        InvokeRepeating("UpdatePath", 0f, waitTime);
    }

    void UpdatePath()
    {
        if ((seeker.IsDone() && !hasFallen) || desistCounter > desistTime)
        {
            if (hasFinished || desistCounter > desistTime)
            {
                destination = new Vector3(
                    rb.position.x + Random.Range(-1 * maximumRadius, maximumRadius),
                    rb.position.y + Random.Range(-1 * maximumRadius, maximumRadius)
                );
                desistCounter = 0;
                if (showDebugMessages) Debug.Log("Desist Counter reseted on UpdatePath()");
            }
            seeker.StartPath(rb.position, destination, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null || hasFallen)
            return;
        hasFinished = (currentWayPoint >= path.vectorPath.Count);
        if (hasFinished)
            return;
        desistCounter++;

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        Debug.DrawRay(rb.position, direction, Color.red, 0f);
        RaycastHit2D raycast = Physics2D.CircleCast(rb.position, .6f, direction, .6f, shouldFallWhenIn);
        if (raycast)
        {
            if (showDebugMessages) Debug.Log("Raycast Collider hit: " + raycast.collider + " | Raycast: " + raycast.point);
            if (!isImpostor)
            {
                Fall();
                return;
            }
            else
            {
                int prob = Random.Range(-1 * (fakeFallProbability - 2), 2);
                if (showDebugMessages) Debug.Log("Probability: " + prob);
                if (prob == 1)
                {
                    Fall();
                    return;
                }
            }
        }
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
            currentWayPoint++;
    }

    void Fall() {
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length - 1)];
        audioSource.Play();
        Invoke("HandleFallRecovery", fallRecoveryTime);
        hasFallen = true;
        desistCounter = 0;
        if (showDebugMessages) Debug.Log("Desist Counter reseted on Fall()");
    }

    void HandleFallRecovery() {
        destination = new Vector3(
            rb.position.x + Random.Range(-1 * maximumRadius, maximumRadius),
            rb.position.y + Random.Range(-1 * maximumRadius, maximumRadius)
        );
        hasFallen = false;
        seeker.StartPath(rb.position, destination, OnPathComplete);
        desistCounter = 0;
        if (showDebugMessages) Debug.Log("Desist Counter reseted on HandleFallRecovery()");
    }
}
