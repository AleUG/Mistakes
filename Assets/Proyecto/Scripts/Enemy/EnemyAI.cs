using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;
    public float chaseDistance = 10f;
    private float originalChaseDistance;
    public float exploreRadius = 10f;
    public float idleDuration = 2.0f; // Tiempo en segundos que el enemigo se queda quieto
    public float maxTimeToReachPoint = 10f; // Tiempo máximo para llegar al punto de exploración
    private bool isExploring = false;
    private Vector3 explorePoint;
    private float idleTimer = 0f;
    private float timeToReachPoint = 0f;

    private EnemyMovement enemyMovement;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Obtén la referencia al script EnemyMovement
        enemyMovement = GetComponent<EnemyMovement>();
        originalChaseDistance = chaseDistance;

        // Inicia la exploración al comienzo
        Explore();
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
                // Si el jugador está dentro del rango de persecución, persigue al jugador
                ChasePlayer();
            }
            else
            {
                // Si no está persiguiendo al jugador, explora el entorno
                Explore();
            }
        }
    }

    void ChasePlayer()
    {
        // Persigue al jugador
        navMeshAgent.SetDestination(player.position);

        // Ajusta la velocidad del NavMeshAgent según la velocidad del script EnemyMovement
        navMeshAgent.speed = enemyMovement.moveSpeed;

        isExploring = false; // Detiene la exploración cuando persigue al jugador
    }

    void Explore()
    {
        if (!isExploring)
        {
            // Encuentra un nuevo punto dentro del radio de exploración
            explorePoint = GetRandomPointInRadius(transform.position, exploreRadius);
            isExploring = true;

            // Reinicia los temporizadores
            idleTimer = 0f;
            timeToReachPoint = 0f;
        }

        // Si ha llegado al punto de exploración, inicia el temporizador de inactividad
        if (Vector3.Distance(transform.position, explorePoint) < 1.0f)
        {
            IdleForDuration();
        }
        else
        {
            // Se mueve hacia el punto de exploración
            navMeshAgent.SetDestination(explorePoint);

            // Verifica si el enemigo tarda demasiado en llegar al punto y elige uno nuevo
            timeToReachPoint += Time.deltaTime;
            if (timeToReachPoint > maxTimeToReachPoint)
            {
                isExploring = false;
            }
        }
    }

    void IdleForDuration()
    {
        // Se queda quieto por el tiempo especificado
        idleTimer += Time.deltaTime;

        // Si ha pasado el tiempo de inactividad, deja de explorar
        if (idleTimer >= idleDuration)
        {
            isExploring = false;
        }
    }

    Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, radius, NavMesh.AllAreas);

        return navHit.position;
    }

    public void ResetChaseDistance()
    {
        chaseDistance = originalChaseDistance;
    }
}
