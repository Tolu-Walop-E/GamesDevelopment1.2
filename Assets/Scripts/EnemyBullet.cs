using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public int damage = 10;

    private Rigidbody rb;

    private void Start()
    {
        // Get the Rigidbody component and set the velocity
        rb = GetComponent<Rigidbody>();

        // Set the velocity of the bullet
        rb.velocity = transform.forward * speed;

        // Destroy the bullet after 'lifeTime' seconds to avoid memory leaks
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // If the player has a health script, reduce their health (not implemented here)

            // Destroy the bullet after hitting the player
            Destroy(gameObject);
        }

        // Destroy the bullet if it collides with anything
        Destroy(gameObject);
    }
}
