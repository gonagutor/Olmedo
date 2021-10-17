using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 17.5f;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 movement;

    private bool run;
    private float internalSpeed;
    private AudioSource audio;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        audio = this.GetComponent<AudioSource>();
        internalSpeed = moveSpeed;
    }

    void Update()
    {
        //Input

        run = Input.GetKey(KeyCode.LeftShift);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 
            || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

        if ((movement.x != 0 || movement.y != 0) && !audio.isPlaying)
        {
            audio.Play();
        }
    }

    void FixedUpdate()
    {
        // Movement
        if (run)
            internalSpeed = runSpeed;
        else
            internalSpeed = moveSpeed;;

        rb.MovePosition(rb.position + movement * internalSpeed * Time.fixedDeltaTime);
    }
}
