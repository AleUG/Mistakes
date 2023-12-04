using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float runSpeed = 5f;
    public float crouchSpeed = 2.5f; // Velocidad al agacharse
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundMask;

    private bool isGrounded;
    public bool isCroushed;
    private Vector3 movementDirection; // Guarda la direcci�n del movimiento

    private Rigidbody rb;
    private Animator animator;
    public AudioSource[] pasosSound;

    public CinemachineVirtualCamera virtualCamera;

    private float stepInterval = 0.55f; // Intervalo de tiempo entre pasos
    private float nextStepTime; // Siguiente momento para reproducir un paso

    public bool isMoving;
    public bool isRunning;
    public bool isMovingAgachado;

    public bool canMove = true;

    private EnemyAI enemyAI;
    CursorLockMode lastCursorLockMode;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        animator = GetComponent<Animator>();

        nextStepTime = Time.time; // Inicializa el pr�ximo tiempo de paso

        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if(Time.timeScale == 0) return;

        if (canMove)
        {
            Cursor.lockState = CursorLockMode.Locked;
            virtualCamera.enabled = true;

            // Obtener la direcci�n en la que mira la c�mara
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f; // Para evitar que el jugador salte hacia arriba

            // Rotar al jugador solo alrededor del eje Y (vertical) en funci�n de la direcci�n de la c�mara
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            // Calcular el movimiento basado en la direcci�n de la c�mara
            movementDirection = cameraForward.normalized * Input.GetAxisRaw("Vertical") +
                                Camera.main.transform.right.normalized * Input.GetAxisRaw("Horizontal");

            if (Physics.CheckSphere(groundCheck.position, 0.1f, groundMask))
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;

                // Detener todos los sonidos de pasos al saltar
                foreach (var pasosAudio in pasosSound)
                {
                    pasosAudio.Stop();
                }
            }

            // Agacharse mientras se mantiene presionado Left Control
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Agacharse();
            }
            else
            {
                DejarDeAgacharse();
            }
        }
        else
        {
            
            Cursor.lockState = CursorLockMode.Locked;
            virtualCamera.enabled = false;

        }
    }
        

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementDirection.sqrMagnitude > 0.001f)
            {
                isMoving = true;
                animator.SetBool("Walk", true);
                movementDirection.Normalize();
                if (Time.time >= nextStepTime)
                {
                    // Reproduce un sonido de paso y establece el pr�ximo tiempo de paso
                    pasosSound[Random.Range(0, pasosSound.Length)].Play();
                    nextStepTime = Time.time + stepInterval;
                }

                // Mover al jugador en la direcci�n del movimiento
                transform.position = transform.position + movementDirection * speed * Time.fixedDeltaTime;

                if (Input.GetKey(KeyCode.LeftShift) && !isCroushed)
                {
                    // Mover al jugador en la direcci�n del movimiento
                    transform.position = transform.position + movementDirection * runSpeed * Time.fixedDeltaTime;
                    stepInterval = 0.35f;
                    isRunning = true;
                }
                else
                {
                    stepInterval = 0.55f;
                    isRunning = false;

                }
            }
            else
            {
                animator.SetBool("Walk", false);
                isMoving = false;
            }
        }
        
    }

    private void Agacharse()
    {
        speed = crouchSpeed; // Reducir velocidad al agacharse
        animator.SetBool("Agacharse", true);
        isCroushed = true;
        stepInterval = 0.65f;
    }

    private void DejarDeAgacharse()
    {
        speed = 2.25f; // Restablecer velocidad al dejar de agacharse
        animator.SetBool("Agacharse", false);
        isCroushed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Escalera"))
        {
            rb.useGravity = false;
            Debug.Log("Accedi� a escalera");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Escalera"))
        {
            rb.useGravity = true;
        }
    }
}
