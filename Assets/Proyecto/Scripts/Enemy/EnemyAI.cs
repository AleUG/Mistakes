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
    public float maxTimeToReachPoint = 10f; // Tiempo m�ximo para llegar al punto de exploraci�n
    private bool isExploring = false;
    private Vector3 explorePoint;
    private float idleTimer = 0f;
    private float timeToReachPoint = 0f;

    private EnemyMovement enemyMovement;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Obt�n la referencia al script EnemyMovement
        enemyMovement = GetComponent<EnemyMovement>();
        originalChaseDistance = chaseDistance;

        // Inicia la exploraci�n al comienzo
        Explore();
    }

    void Update()
    {
        // Verifica si el enemigo est� activo a trav�s del script EnemyMovement
        if (!enemyMovement.isActive) return;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseDistance)
            {
                // Si el jugador est� dentro del rango de persecuci�n, persigue al jugador
                ChasePlayer();
            }
            else
            {
                // Si no est� persiguiendo al jugador, explora el entorno
                Explore();
            }
        }
    }

    void ChasePlayer()
    {
        // Persigue al jugador
        navMeshAgent.SetDestination(player.position);

        // Ajusta la velocidad del NavMeshAgent seg�n la velocidad del script EnemyMovement
        navMeshAgent.speed = enemyMovement.moveSpeed;

        isExploring = false; // Detiene la exploraci�n cuando persigue al jugador
    }

    void Explore()
    {
        if (!isExploring)
        {
            // Encuentra un nuevo punto dentro del radio de exploraci�n
            explorePoint = GetRandomPointInRadius(transform.position, exploreRadius);
            isExploring = true;

            // Reinicia los temporizadores
            idleTimer = 0f;
            timeToReachPoint = 0f;
        }

        // Si ha llegado al punto de exploraci�n, inicia el temporizador de inactividad
        if (Vector3.Distance(transform.position, explorePoint) < 1.0f)
        {
            IdleForDuration();
        }
        else
        {
            // Se mueve hacia el punto de exploraci�n
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
