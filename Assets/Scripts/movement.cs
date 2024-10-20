using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    private CharacterController controller;
    private Collider playerCollider;  
    private Vector3 moveVector;
    private float verticalVelocity;
    private int jumpCounter = 2;
    public Collider[] AttackHitbox;
    public float moveSpeed = 5f;
    private Vector3 movementCheck;

    private bool isRespawning = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();  
    }

    private void Update()
    {
        if (isRespawning) return;


        movementCheck.x = Input.GetAxisRaw("Horizontal");  


        movementCheck.y = 0f;  
        movementCheck.z = 0f;  


        if (movementCheck.x != 0)
        {
            RotatePlayer(movementCheck.x);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            TriggerAttack(AttackHitbox[0]);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TriggerAttack(AttackHitbox[1]);
        }


        if (controller.isGrounded)
        {
            verticalVelocity = -1;
            jumpCounter = 2;

            if (Input.GetKeyDown(KeyCode.W) && jumpCounter > 0)
            {
                verticalVelocity = 10;
                jumpCounter -= 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W) && jumpCounter > 0)
        {
            verticalVelocity = 10;
            jumpCounter -= 1;
        }
        else
        {
            verticalVelocity -= 14 * Time.deltaTime;
        }

        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * moveSpeed;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
    }


    void RotatePlayer(float directionX)
    {
        if (directionX > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);  // Face right
        }
        else if (directionX < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);  // Face left
        }
    }

    // Attack logic
    private void TriggerAttack(Collider col)
    {
        Debug.Log(col.name);
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
            if (c.transform.parent.parent == transform)
            {
                continue;
            }
            else
            {
                Debug.Log(c.name);
            }
        }
    }


    public void Respawn(Vector3 respawnPosition)
    {
        Debug.Log("Respawning player to position: " + respawnPosition);

        isRespawning = true;


        if (playerCollider != null)
        {
            playerCollider.enabled = false;  // Disable the player's collider
        }


        transform.position = respawnPosition;


        verticalVelocity = 0;

        Debug.Log("Player respawn complete at position: " + transform.position);


        Invoke(nameof(EnableMovement), 0.1f);
    }


    private void EnableMovement()
    {
        isRespawning = false;


        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
    }
}
