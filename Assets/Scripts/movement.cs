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

   private void Start()
   {
        controller = GetComponent<CharacterController>();
   }

   private void Update()
   {
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
