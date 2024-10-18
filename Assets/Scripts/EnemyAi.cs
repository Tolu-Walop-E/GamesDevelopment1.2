using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();


        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) Attacking();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {

            agent.SetDestination(new Vector3(walkPoint.x, transform.position.y, transform.position.z));
        }


        if (Vector3.Distance(transform.position, new Vector3(walkPoint.x, transform.position.y, transform.position.z)) < 1f)
        {
            walkPointSet = false;
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

        agent.SetDestination(new Vector3(player.position.x, transform.position.y, transform.position.z));
    }

    private void Attacking()
    {

        agent.SetDestination(transform.position);


        Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.LookAt(lookAtPosition);

        if (!alreadyAttacked)
        {

            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();


            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 xAxisDirection = new Vector3(directionToPlayer.x, 0, 0);

            rb.AddForce(xAxisDirection * 32f, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
