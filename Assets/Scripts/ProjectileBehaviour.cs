using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public static int noProjectiles = 0;
    private bool isOriginal = true;
    private Renderer renderer;
    public Transform player;
    private Vector3 initialDirection;
    public int damage = 10;

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
            Vector3 playerForwardDirection = player.forward;
            initialDirection = new Vector3(playerForwardDirection.z, 0, playerForwardDirection.x).normalized;
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
        
        
        transform.position += initialDirection * speed * Time.deltaTime;
        
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyDamage enemyDamage = other.gameObject.GetComponent<EnemyDamage>();
            enemyDamage.TakeDamage(damage);
            Debug.Log("ENEMY HIT");
            Destroy(gameObject);
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            Debug.Log("OTHER HIT");
            Destroy(gameObject);
        }
    }
    

}        