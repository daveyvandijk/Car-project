using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oneMORE : MonoBehaviour
{
    [Header("Raycast Fire")]
    public Transform[] raycastPoints; 
    public float targetHeight = 2.0f; 
    public float floatingForce = 10.0f; 
    public float damping = 2.0f; 
    public float angularDamping = 2.0f;
    
    [Header("movement")]
    public float moveSpeed = 5.0f; 
    public float acceleration = 2.0f; 
    public float deceleration = 3.0f; 
    
    
    private Rigidbody rb;
    private float currentSpeed = 0.0f; 
    private float input; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.freezeRotation = true; 
    }

    void FixedUpdate()
    {
        HandleFloating();
        HandleMovement();
        StabilizeRotation();
    }

    
    void HandleFloating()
    {
        if (raycastPoints.Length != 4)
        {
            Debug.LogError("Er moeten precies 4 raycast punten zijn.");
            return;
        }

        Vector3 totalForce = Vector3.zero;

        foreach (Transform raycastPoint in raycastPoints)
        {
            RaycastHit hit;
            if (!Physics.Raycast(raycastPoint.position, Vector3.down, out hit)) continue;
            
            float currentHeight = hit.distance;
            float heightDifference = targetHeight - currentHeight;
            Vector3 upwardForce = Vector3.up * (heightDifference * floatingForce);
            totalForce += upwardForce;

            Debug.DrawRay(raycastPoint.position, Vector3.down * currentHeight, Color.red);
        }
        Vector3 dampingForce = -rb.velocity * damping;
        rb.AddForce(totalForce + dampingForce, ForceMode.Acceleration);
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

    void StabilizeRotation()
    {
        // Verminder de ongewenste rotatie met een dempingsfactor
        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, angularDamping * Time.deltaTime);
    }
}
