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
    public bool isKey;

    private bool isOpen = false;

    private AudioSource audioDoorOpen;
    private AudioSource audioDoorClose;

    private Animator animator;
    private Inventario inventario;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        inventario = FindObjectOfType<Inventario>();

        //AudioSources
        audioDoorOpen = GameObject.Find("DoorAudioOpen").GetComponent<AudioSource>();
        audioDoorClose = GameObject.Find("DoorAudioClose").GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    public void InteractArmario()
    {
        if (isArmario)
        {
            animator.SetTrigger("Open");

            if (isEnter == true)
            {
                playerMovement.enabled = false;

                virtualCamera.gameObject.SetActive(true);
            }
            else if (isEnter == false)
            {
                playerMovement.enabled = true;

                virtualCamera.gameObject.SetActive(false);
            }

            if (isEnter == false)
            {
                isEnter = true;
            }
            else if (isEnter == true)
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

    public void InteractKey()
    {
        if (isKey)
        {
            // Aquí, deberías activar la mecánica de llaves y puertas bloqueadas.
            Key keyScript = GetComponent<Key>();
            if (keyScript != null)
            {
                keyScript.Interact(); // Agrega un método en la clase Key para manejar la interacción.
            }

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
                audioDoorClose.Play();
            }
            else
            {
                animatorDoor.SetTrigger("Open");
                isOpen = true;
                audioDoorOpen.Play();
            }
        }
    }

    // Nuevo método para obtener los materiales asignados.
    public Material[] GetAssignedMaterials()
    {
        // Puedes personalizar esta lógica según el tipo de objeto interactuable.
        // En este ejemplo, simplemente devolvemos los materiales del primer renderer.
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.materials;
        }
        else
        {
            // Devuelve un array vacío si no hay renderer.
            return new Material[0];
        }
    }
}
