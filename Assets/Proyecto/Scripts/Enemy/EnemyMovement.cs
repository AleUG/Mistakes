using UnityEngine;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        originalMoveSpeed = moveSpeed;
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
                    // Código para seguir al jugador
                    Vector3 moveDirection = directionToPlayer.normalized;
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    animator.SetBool("Walk", true);

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
}