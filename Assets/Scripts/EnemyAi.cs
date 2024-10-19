using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;  // Reference to your projectile prefab

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float accuracy = 100f;  // Accuracy percentage (100 = perfect, lower = less accurate)

    private float checkInterval = 0.5f;  // Reduce how often we check for player
    private float checkTimer = 0f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        checkTimer += Time.deltaTime;

        // Perform range checks only every `checkInterval`
        if (checkTimer >= checkInterval)
        {
            checkTimer = 0f;
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        }

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) Attacking();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            Vector3 targetPosition = new Vector3(walkPoint.x, transform.position.y, transform.position.z);
            agent.SetDestination(targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                walkPointSet = false;
            }
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        if (Physics.Raycast(new Vector3(walkPoint.x, transform.position.y, walkPoint.z), -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        agent.SetDestination(targetPosition);
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);

        Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.LookAt(lookAtPosition);

        if (!alreadyAttacked)
        {
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);

            // Adjust projectile size based on enemy size
            projectileInstance.transform.localScale = transform.localScale * 0.5f;

            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();

            // Calculate direction to the player with some inaccuracy
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // Introduce inaccuracy by adding random deviation
            Vector3 inaccurateDirection = AddInaccuracy(directionToPlayer, accuracy);

            // Reset velocity (in case of pooling)
            rb.velocity = Vector3.zero;

            // Apply force to the projectile with inaccuracy
            rb.AddForce(inaccurateDirection * 32f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    // Function to add inaccuracy to the projectile's direction
    private Vector3 AddInaccuracy(Vector3 originalDirection, float accuracy)
    {
        // Convert accuracy percentage to an inaccuracy range
        float inaccuracyFactor = (100f - accuracy) / 100f;

        // Add random offset to the X and Y directions (leave Z unchanged if it's a 2D plane)
        float randomOffsetX = Random.Range(-inaccuracyFactor, inaccuracyFactor);
        float randomOffsetY = Random.Range(-inaccuracyFactor, inaccuracyFactor);

        // Modify the original direction with inaccuracy
        Vector3 inaccurateDirection = new Vector3(originalDirection.x + randomOffsetX, originalDirection.y + randomOffsetY, originalDirection.z);

        // Normalize the resulting vector to maintain the original magnitude
        return inaccurateDirection.normalized;
    }
}
