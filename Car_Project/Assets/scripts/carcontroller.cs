using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class carcontroller : MonoBehaviour
{
    
    
    [Header("movement")]
    public float moveSpeed = 5.0f; 
    public float acceleration = 2.0f; 
    public float deceleration = 3.0f; 
    private Rigidbody rb;
    private float currentSpeed = 0.0f; 
    private float input; 
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        HandleMovement();
        HandleFloating();
    }

    void HandleFloating()
    {
        
    }
    
    void HandleMovement()
    {
       
        input = Input.GetAxis("Vertical"); 

        if (input != 0)
        {
            
            currentSpeed += input * acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -moveSpeed, moveSpeed);
        }
        else
        {
           
            if (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                if (currentSpeed < 0) currentSpeed = 0; 
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += deceleration * Time.deltaTime;
                if (currentSpeed > 0) currentSpeed = 0; 
            }
        }

        
        Vector3 move = transform.right * currentSpeed;
        rb.MovePosition(rb.position + move * Time.deltaTime);
    }
}
