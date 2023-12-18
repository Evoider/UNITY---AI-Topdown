// --------------------------------------- //
// --------------------------------------- //
//  Creation Date: 12/12/23
//  Description: AI - Topdown
// --------------------------------------- //
// --------------------------------------- //

using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    public Transform ObjectToMove;
    
    public Vector2 MoveInput { get; set; }
    
    [Header("Stats")]
    public float Speed = 10f;
    private float _defaultSpeed = 5f;

    private Rigidbody2D _rb;
    private void Start()
    {
        _rb = ObjectToMove.GetComponent<Rigidbody2D>();
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    public void Move(float speed)
    {
        Speed = speed;
        _rb.MovePosition(_rb.position + MoveInput * Speed * Time.fixedDeltaTime);
    }
    
    
    
    public void SetAnimationSpeed(Animator animator)
    {
        // Calculate the speed based on fire rate and adjust the speed of the animator
        animator.speed = Speed / _defaultSpeed;
    }
    public void ResetAnimationSpeed(Animator animator)
    {
        animator.speed = 1;
    }
}