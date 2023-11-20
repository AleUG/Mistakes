using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float originalMoveSpeed = 3.5f;
    public float moveSpeed = 3.5f;
    public float followDistance = 5.0f;
    public float rotationSpeed = 10f;
    public float attackDistance = 2.0f;

    private bool isAttacking = false;
    private bool isCooldown = false;
    public float attackCooldown = 0.5f;

    private Transform player;
    private Animator animator;

    public bool canAttack;
    public bool isActive = true;

    private EnemyAI enemyAI;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        originalMoveSpeed = moveSpeed;

        // Agrega esta línea para obtener el componente EnemyAI
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.enabled = false;
    }

    void FixedUpdate()
    {
        if (!isActive) return;

        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;

            if (!isAttacking)
            {
                if (directionToPlayer.magnitude <= followDistance)
                {
                    // Desactiva el movimiento directo
                    enemyAI.enabled = true;

                    if (directionToPlayer.magnitude <= attackDistance && canAttack == true)
                    {
                        isAttacking = true;
                        animator.SetBool("Ataque", true);
                        moveSpeed = 0f;
                    }
                    else
                    {
                        animator.SetBool("Ataque", false);
                    }
                }
                else
                {
                    animator.SetBool("Walk", false);
                    animator.SetBool("Ataque", false);

                    // Activa el movimiento AI
                    enemyAI.enabled = true;
                }
            }
            else
            {
                // Enfriamiento después del ataque
                if (!isCooldown)
                {
                    isCooldown = true;
                    Invoke(nameof(FinishAttack), attackCooldown);
                }
            }
        }
    }

    private void FinishAttack()
    {
        moveSpeed = originalMoveSpeed;
        isAttacking = false;
        isCooldown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            Animator doorAnimator = other.GetComponent<Animator>();
            Interactable interactDoor = other.GetComponent<Interactable>();

            if (doorAnimator != null)
            {
                doorAnimator.SetTrigger("Open");
                interactDoor.isOpen = true;
            }
            else
            {
                interactDoor.isOpen = false;
            }
        }
    }

}