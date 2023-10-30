using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignPlayerToGround : MonoBehaviour
{
    public float alignSpeed = 2.0f;  // Speed of alignment
    private Rigidbody rb;
    [SerializeField] float AlignCheckDist;
    private float targetRotation;  // Target rotation

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
        bool isGrounded = Physics.Raycast(rb.position, Vector3.down, out hit, AlignCheckDist);

        if (isGrounded)
        {
            // Get the angle of the surface using the normal
            Vector3 groundNormal = hit.normal;
            Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, groundNormal);
            Quaternion rotation = Quaternion.LookRotation(projectedForward, groundNormal);
            rb.rotation = Quaternion.Slerp(rb.rotation, rotation, alignSpeed * Time.deltaTime);
        }
        else
        {
            // If not on the ground, return to default rotation
            Quaternion defaultRotation = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 0f);
            rb.rotation = Quaternion.Slerp(rb.rotation, defaultRotation, alignSpeed * Time.deltaTime);
        }
    }
}