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

    private Rigidbody rb;
    private bool isGrounded;
    private bool isCroushed;
    private Vector3 movementDirection; // Guarda la dirección del movimiento

    private Animator animator;
    public AudioSource[] pasosSound;

    private float stepInterval = 0.55f; // Intervalo de tiempo entre pasos
    private float nextStepTime; // Siguiente momento para reproducir un paso

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        animator = GetComponent<Animator>();

        nextStepTime = Time.time; // Inicializa el próximo tiempo de paso
    }

    private void Update()
    {
        // Obtener la dirección en la que mira la cámara
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // Para evitar que el jugador salte hacia arriba

        // Rotar al jugador solo alrededor del eje Y (vertical) en función de la dirección de la cámara
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

        // Calcular el movimiento basado en la dirección de la cámara
        movementDirection = cameraForward.normalized * Input.GetAxis("Vertical") +
                            Camera.main.transform.right.normalized * Input.GetAxis("Horizontal");

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

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

    private void FixedUpdate()
    {
        if (movementDirection.sqrMagnitude > 0.001f)
        {
            animator.SetBool("Walk", true);
            movementDirection.Normalize();
            if (Time.time >= nextStepTime)
            {
                // Reproduce un sonido de paso y establece el próximo tiempo de paso
                pasosSound[Random.Range(0, pasosSound.Length)].Play();
                nextStepTime = Time.time + stepInterval;
            }

            // Mover al jugador en la dirección del movimiento
            transform.position = transform.position + movementDirection * speed * Time.fixedDeltaTime;

            if (Input.GetKey(KeyCode.LeftShift) && !isCroushed)
            {
                // Mover al jugador en la dirección del movimiento
                transform.position = transform.position + movementDirection * runSpeed * Time.fixedDeltaTime;
                stepInterval = 0.35f;
            }
            else
            {
                stepInterval = 0.55f;
            }
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }

    private void Agacharse()
    {
        speed = crouchSpeed; // Reducir velocidad al agacharse
        animator.SetBool("Agacharse", true);
        isCroushed = true;
    }

    private void DejarDeAgacharse()
    {
        speed = 2.75f; // Restablecer velocidad al dejar de agacharse
        animator.SetBool("Agacharse", false);
        isCroushed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Escalera"))
        {
            rb.useGravity = false;
            speed = 6f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Escalera"))
        {
            rb.useGravity = true;
            speed = 5f;
        }
    }
}
