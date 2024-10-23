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
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float accuracy = 100f;

    private float checkInterval = 0.5f;
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
            // Restricting movement to X-axis (keeping Y and Z constant)
            Vector3 targetPosition = new Vector3(walkPoint.x, transform.position.y,0);
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
        // Restrict walkPoint to X-axis, keeping Y and Z constant
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, 0);

        if (Physics.Raycast(new Vector3(walkPoint.x, transform.position.y, 0), -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        // Restricting chase movement to X-axis (keeping Y and Z constant)
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, 0);
        agent.SetDestination(targetPosition);
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);

        Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y,0);
        transform.LookAt(lookAtPosition);

        if (!alreadyAttacked)
        {
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);

            projectileInstance.transform.localScale = transform.localScale * 0.5f;

            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();

            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            Vector3 inaccurateDirection = AddInaccuracy(directionToPlayer, accuracy);

            rb.velocity = Vector3.zero;

            rb.AddForce(inaccurateDirection * 32f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private Vector3 AddInaccuracy(Vector3 originalDirection, float accuracy)
    {
        float inaccuracyFactor = (100f - accuracy) / 100f;

        float randomOffsetX = Random.Range(-inaccuracyFactor, inaccuracyFactor);
        float randomOffsetY = Random.Range(-inaccuracyFactor, inaccuracyFactor);

        Vector3 inaccurateDirection = new Vector3(originalDirection.x + randomOffsetX, originalDirection.y + randomOffsetY, 0);

        return inaccurateDirection.normalized;
    }
}
