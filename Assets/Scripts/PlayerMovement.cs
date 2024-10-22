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
        rb.AddForce(movement * speed * Time.fixedDeltaTime);

        Vector3 currentPosition = rb.position;
        currentPosition.z = lockedZPosition;  // Lock Z position
        rb.position = currentPosition;
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
            TriggerAttack(AttackHitbox[0]);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            TriggerAttack(AttackHitbox[1]);
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

    void EnableMovement()
    {
        isRespawning = false;  // Re-enable movement after respawning
    }
}
