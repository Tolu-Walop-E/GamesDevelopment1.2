using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchCube : MonoBehaviour
{
    public float bounceHeight = 5f;  // The height to which the player will be launched
    public string playerTag = "Player";  // Tag to identify the player object
    public float fallSpeed = 9.8f;  // Simulated gravity for the player falling back down

    private Transform playerTransform;
    private bool isLaunching = false;
    private float launchSpeed = 0f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag))
        {
            // Get the player's transform
            playerTransform = collision.collider.transform;

            // Set launch speed based on bounce height
            launchSpeed = Mathf.Sqrt(2 * fallSpeed * bounceHeight);  // Calculate velocity needed to reach the bounce height
            isLaunching = true;  // Start launching the player
        }
    }

    void Update()
    {
        // If player is being launched, move them upward
        if (isLaunching && playerTransform != null)
        {
            // Move the player upwards
            playerTransform.position += new Vector3(0, launchSpeed * Time.deltaTime, 0);

            // Simulate gravity (reduce launch speed over time to simulate the player coming back down)
            launchSpeed -= fallSpeed * Time.deltaTime;

            // Stop launching when launchSpeed is below zero (falling back to the ground)
            if (launchSpeed <= 0f)
            {
                isLaunching = false;
            }
        }
    }
}
