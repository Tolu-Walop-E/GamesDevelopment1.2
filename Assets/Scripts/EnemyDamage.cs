using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int totalHealth = 30;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    
    public void TakeDamage(int damage)
    {
        totalHealth -= damage;

        if (totalHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
