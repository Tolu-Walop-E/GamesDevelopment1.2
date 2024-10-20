using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    // Start is called before the first frame update
   private CharacterController controller;
   private Vector3 moveVector;
   private float verticalVelocity;
   private int jumpCounter = 2;
   public Collider[] AttackHitbox;
   public float moveSpeed = 5f;
   private Vector3 movementCheck;

   private void Start()
   {
        controller = GetComponent<CharacterController>();
   }

   private void Update()
   {
        // Get input from the player for horizontal movement (X-axis)
        movementCheck.x = Input.GetAxisRaw("Horizontal");  // Left (-1) and Right (1)

        // Zero out the other axes since movement is restricted to X-axis
        movementCheck.y = 0f;  // No movement on Y-axis (for 3D)
        movementCheck.z = 0f;  // No movement on Z-axis

        // If the character is moving, rotate it to face the correct direction
        if (movementCheck.x != 0)
        {
            RotatePlayer(movementCheck.x);
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            TriggerAttack(AttackHitbox[0]);
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            TriggerAttack(AttackHitbox[1]);
        }
        if(controller.isGrounded)
        {
            verticalVelocity = -1;
            jumpCounter = 2;

            if(Input.GetKeyDown(KeyCode.W) && jumpCounter > 0)
            {
                verticalVelocity = 10;
                jumpCounter -= 1;
            }
        }
        else if(Input.GetKeyDown(KeyCode.W) && jumpCounter > 0)
        {
            verticalVelocity = 10;
            jumpCounter -= 1;
        }
        else
        {
            verticalVelocity -= 14 * Time.deltaTime;
        }

        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * 5;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector*Time.deltaTime);
   }

   void RotatePlayer(float directionX)
    {
        if (directionX > 0) 
        {
            // Moving right, rotate to face right (Quaternion.identity = no rotation, facing forward)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (directionX < 0) 
        {
            // Moving left, rotate 180 degrees to face left
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

   private void TriggerAttack (Collider col)
   {    Debug.Log(col.name);
        Collider[] cols = Physics.OverlapBox(col.bounds.center,col.bounds.extents,col.transform.rotation,LayerMask.GetMask("Hitbox"));
        foreach(Collider c in cols)
        {
            if(c.transform.parent.parent == transform)
            {
                continue;
            }
            else
            {
                Debug.Log(c.name);
            }
        }
   }
}
