using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum AILevel
{
    Dumb = 5,
    Average = 8,
    Smart = 10
}

public class AIPlayer : MonoBehaviour
{
    Rigidbody rb;
    Character character;

    [SerializeField, Tooltip("Obstacle avoidance level")] AILevel aiLevel;

    [SerializeField] private float forwardSpeed;
    [SerializeField] private float horizontalSpeed;
    private int horizontalWay;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.PlayerState == State.Running)
        {
            Movement();
            AvoidObstacle();
        }
    }

    private void AvoidObstacle()
    {
        horizontalWay = 0;

        RaycastHit _hit;
        if (rb.SweepTest(transform.forward, out _hit, (int)aiLevel))
        {
            if (_hit.transform.CompareTag("Obstacle") || _hit.transform.CompareTag("Block"))
            {
                RaycastHit _hit2;
                if (rb.SweepTest(transform.right, out _hit2, (int)aiLevel))
                {
                    if (!_hit2.transform.CompareTag("Obstacle") || !_hit2.transform.CompareTag("Border"))
                        horizontalWay = -1;
                    else
                        horizontalWay = 1;
                }
            }
        }
    }

    private void Movement()
    {
        Vector3 _forwardVelocity = transform.forward * forwardSpeed;
        Vector3 _horizontalVelocity = transform.right * horizontalWay * horizontalSpeed;
        Vector3 _finalVelocity = _forwardVelocity + _horizontalVelocity;

        rb.MovePosition(rb.position + _finalVelocity);

        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 1))
        {
            if (_hit.transform.CompareTag("Rotating Platform"))
            {
                Vector3 _movementVector = Vector3.Lerp(rb.position, (new Vector3(0, rb.position.y, rb.position.z) + _forwardVelocity), Time.deltaTime * (int)aiLevel / 2);
                rb.MovePosition(_movementVector);
            }
        }        
            
    }
}
