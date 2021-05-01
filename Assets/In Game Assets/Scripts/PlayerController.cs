using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Character character;

    float swerveAmount;
    [SerializeField] private float sensitivity;
    [SerializeField] private float maxSwerveAmount;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float borderLimit;

    float lastFrameMousePosX;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
    }

    private void FixedUpdate()
    {
#if UNITY_STANDALONE_WIN
        if(Input.GetMouseButtonDown(0))
        {
            if (character.PlayerState == State.Idle)
                EventManager.StartGame();

            lastFrameMousePosX = Input.mousePosition.x;
        }
        else if(Input.GetMouseButton(0))
        {            
            float _inputDifference = Input.mousePosition.x - lastFrameMousePosX;
            swerveAmount = Mathf.Clamp((_inputDifference) * Time.fixedDeltaTime * sensitivity, -maxSwerveAmount, maxSwerveAmount);

            // Prevent falling off from borders of the platform.
            RaycastHit _hit;
            if (rb.SweepTest(transform.right * _inputDifference, out _hit, borderLimit))
            {
                if (_hit.transform.CompareTag("Border"))                                    
                    swerveAmount = 0;
            }

            lastFrameMousePosX = Input.mousePosition.x;
        }
        else if(Input.GetMouseButtonUp(0))        
            swerveAmount = 0;        

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            
            switch(_touch.phase)
            {
                case TouchPhase.Began:
                    if (character.PlayerState == State.Idle)
                        character.PlayerState = State.Running;
                    break;
                case TouchPhase.Moved:
                    swerveAmount = Mathf.Clamp(_touch.deltaPosition.x, -maxSwerveAmount, maxSwerveAmount);
                    break;
                case TouchPhase.Canceled:
                case TouchPhase.Ended:
                    swerveAmount = 0;
                    break;
            }            
        }
#endif

        if (character.PlayerState == State.Running)
            Movement();
    }    

    public void Movement()
    {        
        Vector3 _forwardVelocity = transform.forward * forwardSpeed;
        Vector3 _horizontalVelocity = transform.right * swerveAmount * sensitivity;
        Vector3 _finalVelocity = _forwardVelocity + _horizontalVelocity;        
        
        rb.MovePosition(rb.position + _finalVelocity);
    }
}
