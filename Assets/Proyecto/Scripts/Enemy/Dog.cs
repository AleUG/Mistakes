using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    private EnemyAI enemyAI;
    private float radioActivación = 5f;
    private Transform playerTransform; // Cambiado a Transform
    private Animator animator;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();
        playerTransform = GameObject.FindWithTag("Player").transform; // Cambiado a Transform
        playerMovement = FindObjectOfType<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanciaAlJugador = Vector3.Distance(transform.position, playerTransform.position);

        if (distanciaAlJugador < radioActivación)
        {
            if (playerMovement.isCroushed)
            {
                enemyAI.SetSleepingDog(true);
                animator.SetBool("Alerta", false);
            }
            else
            {
                enemyAI.SetSleepingDog(false);
                animator.SetBool("Alerta", true);
            }

        }
        else
        {
            enemyAI.SetSleepingDog(true);
            animator.SetBool("Alerta", false);
        }
    }
}
