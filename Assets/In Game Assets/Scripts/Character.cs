using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Running,    
    Won
}

public class Character : MonoBehaviour
{
    [SerializeField] State playerState;    

    Vector3 startPos;
    Vector3 lastCheckpoint;

    Animator anim;
    Rigidbody rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        lastCheckpoint = transform.position;

        EventManager.GameReset += SetDefaults;
        EventManager.ObstacleHit += GoCheckpoint;
    }

    private void SetDefaults()
    {
        transform.position = startPos;
        PlayerState = State.Idle;
    }

    private void GoCheckpoint()
    {
        transform.position = lastCheckpoint;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Obstacle"))        
            EventManager.HitObstacle();
        else if(collision.collider.CompareTag("Stick"))
        {
            Vector3 _forceDirection = transform.position - collision.contacts[0].point;
            _forceDirection.y = 0;
            
            rb.AddForce(_forceDirection * 8, ForceMode.Impulse);
        }
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            PlayerState = State.Won;
            EventManager.WinGame();
        }
        else if (other.CompareTag("Checkpoint"))
            lastCheckpoint = other.transform.position;


    }

    public State PlayerState
    {
        get { return playerState; }
        set
        {
            playerState = value;
            anim.SetInteger("state", (int)playerState);
        }
    }
}
