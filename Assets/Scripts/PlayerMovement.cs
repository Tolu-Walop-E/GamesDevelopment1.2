using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public Vector2 jump;
    public float jumpSpeed = 5f;
    public float speed;
    private int maxJumps = 2;
    public int jumpCount;
    private Vector3 movementCheck;
    private Rigidbody rb;
    private float lockedZPosition = -0.344f;
    public Collider[] AttackHitbox;
    private bool isRespawning = false;  // Flag to control movement when respawning
    public GameObject PlayerProjectile; 

    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (jumpCount < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            jumpCount++;
        }
    }

    void FixedUpdate()
    {
        if (isRespawning)
        {
            // Skip movement logic while respawning
            return;
        }

        movementCheck.x = Input.GetAxisRaw("Horizontal");  
        movementCheck.y = 0f;  
        movementCheck.z = 0f;  

        if (movementCheck.x != 0)
        {
            RotatePlayer(movementCheck.x);
        }

        

        Vector3 movement = new Vector3(moveValue.x, 0.0f, 0.0f);  // Ensure Z movement is 0
        Vector3 newPosition = rb.position + (movement * speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    void Update()
    {
        if (isRespawning)
        {
            // Skip movement logic while respawning
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            TriggerAttack(AttackHitbox[0], 0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TriggerAttack(AttackHitbox[1], 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //instantiate player projectile prefab and set initial position to player position
            Instantiate(PlayerProjectile, transform.position, Quaternion.identity);
        }
    }

    // Attack logic
    private void TriggerAttack(Collider col, int type)
    {
        Debug.Log(col.name);
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Enemy"));
        foreach (Collider c in cols)
        {
            if (c.transform.parent == transform)
            {
                continue;
            }
            else
            {
                if (type == 0)
                {
                    c.SendMessageUpwards("TakeDamage", 10);
                }
                else if (type == 1)
                {
                    c.SendMessageUpwards("TakeDamage", 15);
                }
                Debug.Log(c.name);
            }
        }
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0; // Reset jump count on landing
        }
    }

    // Call this method to respawn the player
    public void Respawn(Vector3 respawnPosition)
    {
        isRespawning = true;  // Disable movement while respawning

        // Reset player position
        rb.position = respawnPosition;

        // Reset player velocity to stop movement after respawn
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Re-enable movement after a short delay (optional)
        Invoke(nameof(EnableMovement), 0.1f);  // Give physics engine a moment to update the position
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.gameObject.CompareTag("Terrain"))
    //    {
    //        Respawn(respawnPosition);
    //    }
    //}

    void EnableMovement()
    {
        isRespawning = false;  // Re-enable movement after respawning
    }
}
