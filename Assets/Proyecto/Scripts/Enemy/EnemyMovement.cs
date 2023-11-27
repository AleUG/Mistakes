using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

        // Agrega esta l�nea para obtener el componente EnemyAI
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
                // Enfriamiento despu�s del ataque
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
            Interactable doorInteractable = other.GetComponent<Interactable>();
            Door door = other.GetComponent<Door>();

            if (doorAnimator != null && !door.isLocked && doorInteractable.isOpen == false)
            {
                doorAnimator.SetTrigger("OpenEnemy");
                doorInteractable.isOpen = true;

                // Obtener la duraci�n de la animaci�n
                AnimationClip[] clips = doorAnimator.runtimeAnimatorController.animationClips;

                // Programar la acci�n para establecer isOpen en false despu�s de la duraci�n de la animaci�n
                StartCoroutine(CloseDoorAfterDelay(2f, doorInteractable));
            }
        }
    }

    private IEnumerator CloseDoorAfterDelay(float delay, Interactable interactable)
    {
        yield return new WaitForSeconds(delay);

        // Establecer isOpen en false despu�s de la animaci�n
        if (interactable != null)
        {
            interactable.isOpen = false;
        }
    }

}