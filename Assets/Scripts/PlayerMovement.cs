using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public Vector2 jump;
    public float jumpSpeed = 5f;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Correct method name and typo fix
    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed , ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
      
    }


    


}