using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCameraArmario;
    public GameObject player;

    public GameObject canvasNote; //Activar el CanvasNotes
    public GameObject activarCollider; //Activar collider de Diálogo

    private PlayerMovement playerMovement;
    public bool isEnter = false;

    public bool isArmario;
    public bool isPila;
    public bool isDoor;
    public bool isKey;
    public bool isLinterna;
    public bool isMovil;
    public bool isObstacle;
    public bool isCajon;
    public bool isNota;

    public bool isOpen = false;

    private AudioSource audioDoorOpen;
    private AudioSource audioDoorClose;
    private AudioSource audioDoorClosed;
    private AudioSource audioCajonOpen;
    private AudioSource audioCajonClose;

    private AudioSource audioNoteOpen;
    private AudioSource audioNoteClose;

    private AudioSource audioPickUp;

    private Animator animator;
    private Inventario inventario;
    private Linterna linterna;
    private Movil movil;
    private EnemyAI enemyAI;
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        inventario = FindObjectOfType<Inventario>();
        linterna = GameObject.FindGameObjectWithTag("Player").GetComponent<Linterna>();
        movil = GameObject.Find("Player").GetComponent<Movil>();
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();

        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();

        if (canvasNote != null)
        {
            canvasNote.SetActive(false);
        }

        //AudioSources
        audioDoorOpen = GameObject.Find("DoorAudioOpen").GetComponent<AudioSource>();
        audioDoorClose = GameObject.Find("DoorAudioClose").GetComponent<AudioSource>();
        audioDoorClosed = GameObject.Find("DoorClosed").GetComponent<AudioSource>();
        audioNoteOpen = GameObject.Find("AudioNoteOpen").GetComponent<AudioSource>();
        audioNoteClose = GameObject.Find("AudioNoteClose").GetComponent<AudioSource>();
        audioPickUp = GameObject.Find("LinternaPickUp").GetComponent<AudioSource>();
        audioCajonOpen = GameObject.Find("CajónOpen").GetComponent<AudioSource>();
        audioCajonClose = GameObject.Find("CajónClose").GetComponent<AudioSource>();

    }

    public void InteractArmario()
    {
        if (isArmario)
        {
            
            animator.SetTrigger("Open");

            if (isEnter)
            {
                playerMovement.canMove = true;
                StartCoroutine(ActivateGameObject());
                enemyAI.SetArmarioEnter(false);

                virtualCameraArmario.gameObject.SetActive(false);

                isEnter = false;
            }
            else
            {
                playerMovement.canMove = false;
                player.SetActive(false);
                enemyAI.SetArmarioEnter(true);

                virtualCameraArmario.gameObject.SetActive(true);

                isEnter = true;
            }
        }
    }

    public bool ArmarioState()
    {
        return isEnter;
    }

    public void InteractPila()
    {
        if (isPila && inventario.pilasActuales < inventario.pilasMáximas)
        {
            inventario.RecogerPila(1);
            audioPickUp.Play();
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
                audioPickUp.Play();
            }

            gameObject.SetActive(false);
        }
    }

    public void InteractDoor()
    {
        if (isDoor)
        {
            Animator animatorDoor = GetComponent<Animator>();
            Door door = GetComponent<Door>();

            // Verifica si la puerta está cerrada con llave.
            if (door.isLocked)
            {
                animatorDoor.SetTrigger("Force");
                audioDoorClosed.Play();
                return;
            }

            if (isOpen == true)
            {
                animatorDoor.SetTrigger("Close");
                isOpen = false;
                audioDoorClose.Play();
            }
            else if (isOpen == false)
            {
                animatorDoor.SetTrigger("Open");
                isOpen = true;
                audioDoorOpen.Play();
            }
        }
    }

    public void InteractLinterna()
    {
        linterna.Unlock();
        audioPickUp.Play();
        gameObject.SetActive(false);
        canvasNote.SetActive(true); // Activar collider de Tutorial
        Destroy(gameObject, 2.0f);
    }

    public void InteractObstacle()
    {
        if (isObstacle)
        {
            Animator animatorObstacle = GetComponent<Animator>();
            Door door = GetComponent<Door>();

            if (door.isLocked)
            {
                audioDoorClosed.Play();
                return;
            }

            if (isOpen == false)
            {
                isOpen = true;
                audioDoorOpen.Play();
                animatorObstacle.SetTrigger("Out");
            }
        }
    }

    public void InteractNote()
    {
        if (isNota)
        {
            PauseManager pauseManager = FindObjectOfType<PauseManager>();
            Animator animator = canvasNote.GetComponent<Animator>();

            // Verifica si el canvasNote está desactivado
            if (!canvasNote.activeSelf)
            {
                canvasNote.SetActive(true);
                audioNoteOpen.Play();

                if (activarCollider != null)
                {
                    activarCollider.SetActive(true);
                }

                Time.timeScale = 0f;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    animator.SetTrigger("End");
                    Invoke(nameof(DeactivateCanvasNote), 0.3f);
                    pauseManager.ResumeGame();
                    audioNoteClose.Play();
                }
            }
        }
    }

    public void InteractCajon()
    {
        if (isCajon)
        {
            Animator animator = GetComponent<Animator>();
            if (isOpen)
            {
                animator.SetTrigger("Close");
                isOpen = false;
                audioCajonClose.Play();
            }
            else
            {
                animator.SetTrigger("Open");
                isOpen = true;
                audioCajonOpen.Play();
            }

        }
    }
    public void InteractMovil()
    {
        if(isMovil)
        {
            audioPickUp.Play();
            movil.Unlock();
       
            if (activarCollider != null)
            {
                activarCollider.SetActive(true);
            }

            gameObject.SetActive(false);
            Destroy(gameObject, 2.0f);
        }
    }


    private void DeactivateCanvasNote()
    {
        canvasNote.SetActive(false);
    }

    private IEnumerator ActivateGameObject()
    {
        yield return new WaitForSeconds(2.0f);

        player.SetActive(true);
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