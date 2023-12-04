using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;
    public float chaseDistance = 10f;
    private float originalChaseDistance;
    private float alucinatingChaseDistance = 12.5f;
    private float dogAlertDistance = 14f;

    public float exploreRadius = 10f;
    public float idleDuration = 2.0f; // Tiempo en segundos que el enemigo se queda quieto
    public float maxTimeToReachPoint = 10f; // Tiempo m�ximo para llegar al punto de exploraci�n

    private bool isExploring = false;
    private bool isAlucinating = false;
    private bool isDogSleeping = false;
    private bool isWalking;
    private bool isEnter;

    private Vector3 explorePoint;
    private float idleTimer = 0f;
    private float timeToReachPoint = 0f;

    private EnemyMovement enemyMovement;
    private PlayerMovement playerMovement;
    private Animator animator;

    private AudioSource audioWalk;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Obt�n la referencia al script EnemyMovement
        enemyMovement = GetComponent<EnemyMovement>();
        originalChaseDistance = chaseDistance;

        audioWalk = GameObject.Find("PasosEnemy").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        // Inicia la exploraci�n al comienzo
        Explore();
    }

    void Update()
    {
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

                MovementChaseDistance();
            }
        }

        if (isWalking)
        {
            audioWalk.Play();
        }
        else
        {
            audioWalk.Stop();
        }
    }

    void ChasePlayer()
    {
        // Persigue al jugador
        navMeshAgent.SetDestination(player.position);

        // Ajusta la velocidad del NavMeshAgent seg�n la velocidad del script EnemyMovement
        navMeshAgent.speed = enemyMovement.moveSpeed;
        
        if (isAlucinating)
        {
            chaseDistance = alucinatingChaseDistance;
        }
        else if(!isDogSleeping)
        {
            chaseDistance = dogAlertDistance;
        }
        else
        {
            chaseDistance = originalChaseDistance;
        }

        isExploring = false; // Detiene la exploraci�n cuando persigue al jugador
        isWalking = true;
        animator.SetBool("Walk", true);
    }

    void Explore()
    {
        if (!isExploring)
        {
            // Encuentra un nuevo punto dentro del radio de exploraci�n
            explorePoint = GetRandomPointInRadius(transform.position, exploreRadius);
            isExploring = true;

            // Solo establece isWalking en true si no est� ya caminando
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("Walk", true);
            }

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
            animator.SetBool("Walk", true);

            // Verifica si el enemigo tarda demasiado en llegar al punto y elige uno nuevo
            timeToReachPoint += Time.deltaTime;
            if (timeToReachPoint > maxTimeToReachPoint)
            {
                isExploring = false;
                isWalking = false;
                animator.SetBool("Walk", false);
            }
        }
    }

    private void MovementChaseDistance()
    {
        if (playerMovement.isMoving && !isAlucinating)
        {
            // Si el jugador est� caminando, ajusta chaseDistance seg�n tu l�gica actual
            chaseDistance = originalChaseDistance;

            if (playerMovement.isRunning)
            {
                chaseDistance = originalChaseDistance * 1.15f;
            }
            else if (playerMovement.isCroushed)
            {
                chaseDistance = originalChaseDistance * 0.75f;
            }
        }
        else if (isAlucinating)
        {
            // Configura chaseDistance durante la alucinaci�n
            chaseDistance = alucinatingChaseDistance;
        }
        else if (!isDogSleeping)
        {
            chaseDistance = dogAlertDistance;
        }
        else
        {
            // Resetea chaseDistance si el jugador no est� caminando
            ResetChaseDistance();
        }

        if (isEnter)
        {
            chaseDistance = 0.05f;
        }
    }

    private void IdleForDuration()
    {
        // Se queda quieto por el tiempo especificado
        idleTimer += Time.deltaTime;

        // Si ha pasado el tiempo de inactividad, deja de explorar
        if (idleTimer >= idleDuration)
        {
            isExploring = false;
            isWalking = false;
            animator.SetBool("Walk", false);
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
        if (playerMovement.isCroushed)
        {
            chaseDistance = originalChaseDistance * 0.35f;
        }
        else
        {
            chaseDistance = originalChaseDistance;
        }
    }

    public void SetAlucinating(bool alucinating)
    {
        isAlucinating = alucinating;
    }

    public void SetSleepingDog(bool sleeping)
    {
        isDogSleeping = sleeping;
    }

    public void SetArmarioEnter(bool armarioEnter)
    {
        isEnter = armarioEnter;
    }

}
