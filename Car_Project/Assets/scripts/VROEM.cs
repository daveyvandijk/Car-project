using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VROEM : MonoBehaviour
{
    public Transform[] wheels;
    public float suspensionRestDist = 1.0f;
    public float springStrength = 2000f;
    public float springDamper = 50f;
    public LayerMask groundLayer;

    private Rigidbody carRigidBody;

    void Start()
    {
        carRigidBody = GetComponent<Rigidbody>();

        if (carRigidBody == null)
        {
            Debug.LogError("Geen Rigidbody gevonden op het voertuig.");
            return;
        }

        carRigidBody.centerOfMass = new Vector3(0, -0.5f, 0);

        if (wheels.Length != 4)
        {
            Debug.LogError("Er moeten precies 4 wielen zijn.");
            return;
        }
    }

    void FixedUpdate()
    {
        foreach (Transform wheel in wheels)
        {
            ApplySuspension(wheel);
        }
    }

    void ApplySuspension(Transform tireTransform)
    {
        RaycastHit hit;

        if (Physics.Raycast(tireTransform.position, -tireTransform.up, out hit, suspensionRestDist * 2, groundLayer))
        {
            Vector3 springDir = tireTransform.up;
            Vector3 tireWorldVel = carRigidBody.GetPointVelocity(tireTransform.position);
            float offset = suspensionRestDist - hit.distance;
            float vel = Vector3.Dot(springDir, tireWorldVel);
            float force = (offset * springStrength) - (vel * springDamper);

            Debug.Log("Kracht voor wiel " + tireTransform.name + ": " + force);

            carRigidBody.AddForceAtPosition(springDir * force, tireTransform.position);
        }
    }
}
