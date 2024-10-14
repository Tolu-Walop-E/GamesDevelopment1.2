using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.up;
    public static int noProjectiles = 0;
    private bool isOriginal = true;
    private Renderer renderer;

    private void Start()
    {

        renderer = GetComponent<Renderer>();
        if (noProjectiles == 0)
        {
            isOriginal = true;
            noProjectiles++;
        }
        else
        {
            isOriginal = false;
        }
    }

    void Update()
    {
        if (isOriginal)
        {
            renderer.enabled = false;
        }
        if (!isOriginal)
        {
            renderer.enabled = true;
            MoveProjectile();
            
        }
    }

    void MoveProjectile()
    {
        Vector3 moveDirection = new Vector3(1, 0, 0);
        transform.position += moveDirection * speed * Time.deltaTime;
        
        // Check if the projectile is out of bounds and destroy clones only
        if (!IsWithinBounds() && !isOriginal)
        {
            Destroy(gameObject);
        }
    }

    bool IsWithinBounds()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > Screen.width ||
            screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            return false;
        }
        return true;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
    }
    
    

}