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
    public float fallRecoveryTime = 1f;
    public LayerMask shouldFallWhenInFront;

    private Seeker seeker;
    private Rigidbody2D rb;

    private Vector2 destination;
    private Path path;
    private int currentWayPoint = 0;
    private bool hasFinished = false;
    private bool hasFallen = false;
    // Start is called before the first frame update
    void Start()
    {
        seeker = this.GetComponent<Seeker>();
        rb = this.GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, waitTime);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && !hasFallen) {
            if (hasFinished)
                destination = new Vector3(
                    rb.position.x + Random.Range(-1 * maximumRadius, maximumRadius),
                    rb.position.y + Random.Range(-1 * maximumRadius, maximumRadius)
                );

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

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        Debug.DrawRay(rb.position, direction, Color.red, 0f);
        if (Physics2D.Raycast(rb.position, direction, 1f))
        {
            Invoke("HandleFallRecovery", 1f);

            return;
        }
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }
    }

    void HandleFallRecovery() {
        destination = new Vector3(
            rb.position.x + Random.Range(-1 * maximumRadius, maximumRadius),
            rb.position.y + Random.Range(-1 * maximumRadius, maximumRadius)
        );
        hasFallen = false;
        seeker.StartPath(rb.position, destination, OnPathComplete);
    }
}