using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public float jumpSpeed = 5f;
    public float speed;
    private int maxJumps = 2;
    public int jumpCount;
    private Vector3 movementCheck;
    private Rigidbody rb;
    private bool isRespawning = false;
    public GameObject PlayerProjectile;
    private Collider playerCollider;

    // Respawn position, controlled from PlayerDamage
    public Vector3 respawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        if (respawnPosition == Vector3.zero)
        {
            respawnPosition = new Vector3(-83, 1, -0.344f); // Default respawn point
        }
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
        if (isRespawning) return;

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
        if (isRespawning) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(PlayerProjectile, transform.position, Quaternion.identity);
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

    // Collision-based respawn
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Respawn(respawnPosition);  // Use the respawnPosition set in PlayerDamage
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0; // Reset jump count on landing
        }
    }

    // Trigger-based respawn (if Respawn is a trigger)
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Respawn(respawnPosition);  
        }
    }

    public void Respawn(Vector3 respawnPosition)
    {
        isRespawning = true;

        // Reset player position
        rb.position = respawnPosition;

        // Reset player velocity to stop movement after respawn
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Invoke(nameof(EnableMovement), 0.1f);
    }

    void EnableMovement()
    {
        isRespawning = false; 
    }
}
