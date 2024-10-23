using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    public Image healthBar;
    public float totalHealth = 100f;
    public Vector3 respawnPosition;

    private PlayerController playerMovement;

    void Start()
    {

        playerMovement = GetComponent<PlayerController>();


        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = transform.position;
        }
    }

    public void TakeDamage(float damage)
    {
        totalHealth -= damage;
        healthBar.fillAmount = totalHealth / 100f;

        if (totalHealth <= 0)
        {
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        Debug.Log("Player is respawning...");


        playerMovement.Respawn(respawnPosition);  

        totalHealth = 100f;  
        healthBar.fillAmount = totalHealth / 100f;  


    }
}
