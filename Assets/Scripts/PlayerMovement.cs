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
    private int maxJumps = 2;
    public int jumpCount;
    public GameObject ProjectilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
    }
    // Correct method name and typo fix
    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (jumpCount < maxJumps)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed , ForceMode.Impulse);
            jumpCount++;
        }
        
        
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);
      
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0; // Reset jump count when landing on the ground
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //instantiate projectile prefab and set initial position to player position
            Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        }
    }

    

}