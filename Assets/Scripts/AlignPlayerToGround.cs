using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//generated partially with ChatGPT
public class AlignPlayerToGround : MonoBehaviour
{
    public float alignSpeed = 2.0f;  // Speed of alignment
    private Rigidbody2D rb;
    private float targetRotation;  // Target angle for alignment

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetRotation = 0f; // Default rotation
    }

    void Update()
    {
        AlignWithGround();
    }

    void AlignWithGround()
    {
        var position = rb.position;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(position.x,position.y), Vector2.down, 5f);

        if (hit.collider != null)
        {
            // Get the angle of the surface using the normal
            float surfaceAngle = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg - 90f;
            targetRotation = surfaceAngle;
        }
        else
        {
            // If not on the ground, return to default rotation
            targetRotation = 0f;
        }

        // Lerp the player's rotation to the target rotation
        float currentRotation = Mathf.LerpAngle(rb.rotation, targetRotation, alignSpeed * Time.deltaTime);
        rb.SetRotation(currentRotation);
    }
}
