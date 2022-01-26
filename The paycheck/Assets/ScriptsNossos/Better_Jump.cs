using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Better_Jump : MonoBehaviour
{
    public float JumpHeight;
    public float TimeToJumpHeight = 2;
    public Vector3 _velocity;

    // Update is called once per frame
    void Update()
    {
        // Calculate the gravity and initial jump velocity values 
        float _jumpGravity;
        float _jumpVelocity;
        _jumpGravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpHeight, 2);
        _jumpVelocity = Mathf.Abs(_jumpGravity) * TimeToJumpHeight;

        // Step update
        Vector3 stepMovement = (_velocity + Vector3.up * -1 * Time.deltaTime * 0.5f) * Time.deltaTime;
        transform.Translate(stepMovement);
        _velocity.y += -1 * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            // When jump button pressed
            _velocity.y = _jumpVelocity;
        }
    }
}