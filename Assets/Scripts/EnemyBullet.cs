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

        rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * speed;

      
        Destroy(gameObject, lifeTime);
    }



    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hit the player
        if (collision.gameObject.CompareTag("Player"))
        {

            PlayerDamage playerDamage =  collision.gameObject.GetComponent<PlayerDamage>();

            if (playerDamage != null)
            {

                playerDamage.TakeDamage(damage);
            }
            
            // Destroy the bullet after hitting the player
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}
