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
                character.PlayerState = State.Running;
            lastFrameMousePosX = Input.mousePosition.x;
        }
        else if(Input.GetMouseButton(0))
        {
            swerveAmount = Mathf.Clamp((Input.mousePosition.x - lastFrameMousePosX) * Time.fixedDeltaTime * sensitivity, -maxSwerveAmount, maxSwerveAmount);
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
