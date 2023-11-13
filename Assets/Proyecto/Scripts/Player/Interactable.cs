using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interactable : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public MeshRenderer playerRender;

    private PlayerMovement playerMovement;
    private bool isEnter = false;

    public bool isArmario;
    public bool isPila;
    public bool isDoor;

    private bool isOpen = false; // Mover la declaración de la variable fuera del método

    private Animator animator;
    private Inventario inventario;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        inventario = FindObjectOfType<Inventario>();
    }

    private void Update()
    {
        if (isArmario)
        {
            if (isEnter)
            {
                playerMovement.enabled = false;

                // Bloquear la rotación de la cámara del jugador.
                playerMovement.SetCameraRotationEnabled(false);

                virtualCamera.gameObject.SetActive(true);
            }
            else
            {
                playerMovement.enabled = true;

                // Habilitar la rotación de la cámara del jugador.
                playerMovement.SetCameraRotationEnabled(true);

                virtualCamera.gameObject.SetActive(false);
            }
        }

    }

    public void InteractArmario()
    {
        if (isArmario)
        {
            animator.SetTrigger("Open");

            if (!isEnter)
            {
                isEnter = true;
            }
            else
            {
                isEnter = false;
            }
        }

    }

    public void InteractPila()
    {
        if (isPila && inventario.pilasActuales < inventario.pilasMáximas)
        {
            inventario.RecogerPila(1);
            gameObject.SetActive(false);
            Destroy(gameObject, 2.0f);
        }
    }

    public void InteractDoor()
    {
        if (isDoor)
        {
            Animator animatorDoor = GetComponent<Animator>();

            if (isOpen)
            {
                animatorDoor.SetTrigger("Close");
                isOpen = false;
            }
            else
            {
                animatorDoor.SetTrigger("Open");
                isOpen = true;
            }
        }
    }
}
