using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;
    public float chaseDistance = 10f;

    private EnemyMovement enemyMovement;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Obtén la referencia al script EnemyMovement
        enemyMovement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        // Verifica si el enemigo está activo a través del script EnemyMovement
        if (!enemyMovement.isActive) return;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseDistance)
            {
                ChasePlayer();
            }
            else
            {
                StopChasing();
            }
        }
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
    }

    void StopChasing()
    {
        navMeshAgent.SetDestination(transform.position);
    }
}
