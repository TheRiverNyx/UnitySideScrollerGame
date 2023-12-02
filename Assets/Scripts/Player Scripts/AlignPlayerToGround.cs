using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AlignPlayerToGround : MonoBehaviour
{
    public float alignSpeed = 1.0f; // Speed of alignment
    private Rigidbody rb;

    [FormerlySerializedAs("AlignCheckDist")] [SerializeField]
    float alignCheckDist;

    private float targetRotation; // Target rotation
    [SerializeField] LayerMask groundLayer; // Specify the ground layer in the inspector
    public float maxSurfaceAngle = 45f; // Maximum allowed surface angle in degrees

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = 0f; // Default rotation
    }

    void FixedUpdate()
    {
        AlignWithGround();
    }

    void AlignWithGround()
    {
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(rb.position, Vector3.down, out hit, alignCheckDist, groundLayer);

        if (isGrounded)
        {
            // Check if the surface angle is within the allowed range
            float surfaceAngle = Vector3.Angle(Vector3.up, hit.normal);
            if (surfaceAngle <= maxSurfaceAngle)
            {
                // Check the gap size or any other criteria specific to your game
                float gapSize = CalculateGapSize(hit);
                
                float gapThreshold = 0.1f; 

                if (gapSize > gapThreshold)
                {
                    // Get the angle of the surface using the normal
                    Vector3 groundNormal = hit.normal;
                    Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, groundNormal);
                    Quaternion rotation = Quaternion.LookRotation(projectedForward, groundNormal);
                    rb.rotation = Quaternion.Slerp(rb.rotation, rotation, alignSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            // If not on the ground, return to default rotation
            Quaternion defaultRotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            rb.rotation = Quaternion.Slerp(rb.rotation, defaultRotation, alignSpeed * Time.deltaTime);
        }
    }

// Add a method to calculate the size of the gap based on the hit information
    float CalculateGapSize(RaycastHit hit)
    {
        // Calculate the gap size based on the distance between the player's position and the hit point
        float gapSize = Vector3.Distance(rb.position, hit.point);

        return gapSize;
    }
}