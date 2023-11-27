using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class DialogueInteract : MonoBehaviour
{
    private bool isPlayerRange = false;
    private bool isDialogueInProgress = false;
    private bool isWriting = false;

    [SerializeField] private GameObject teclaContinue;
    [SerializeField] private GameObject dialoguePanelnteract;
    [SerializeField] private TextMeshProUGUI dialogueTextInteract;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;

    public GameObject llave;

    private BoxCollider boxCollider;

    private int lineIndex;
    private Coroutine typingCoroutine;

    [SerializeField] private float typingTime = 0.05f;

    private PlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;

    [System.Serializable]
    public class DialogueGroup
    {
        [TextArea(4, 6)] public string[] dialogueLines;
    }

    public DialogueGroup[] dialogueGroups;

    private int currentGroupIndex; // Índice del grupo actual

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        playerMovement = FindObjectOfType<PlayerMovement>();
        playerInteraction = FindObjectOfType<PlayerInteraction>();

        dialoguePanelnteract.SetActive(false);

        playerMovement.canMove = true;
    }

    private void Update()
    {
        if (isPlayerRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueInProgress)
            {
                StartDialogue();
            }
            else if (isWriting)
            {
                // Si está escribiendo y se presiona E, mostrar todo el texto y detener la animación de escritura
                StopCoroutine(typingCoroutine);
                dialogueTextInteract.text = dialogueGroups[currentGroupIndex].dialogueLines[lineIndex];
                isWriting = false;
                isPlayerRange = true;
                teclaContinue.SetActive(true);
            }
            else
            {
                if (lineIndex < dialogueGroups[currentGroupIndex].dialogueLines.Length - 1)
                {
                    NextDialogueLine();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }


    private void StartDialogue()
    {
        isDialogueInProgress = true;
        isPlayerRange = false;

        dialoguePanelnteract.SetActive(true);
        dialogueTextInteract.text = string.Empty;
        boxCollider.enabled = false;

        if (playerMovement != null)
        {
            playerMovement.canMove = false;
        }

        lineIndex = 0; // Iniciar con el primer elemento del grupo actual

        typingCoroutine = StartCoroutine(ShowLines());

        if (playerInteraction != null)
        {
            playerInteraction.enabled = false;
        }
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        dialogueTextInteract.text = string.Empty;
        typingCoroutine = StartCoroutine(ShowLines());

        if (dialoguePanelnteract == true)
        {
            if (playerMovement != null)
            {
                playerMovement.canMove = false;
            }
        }
    }

    private IEnumerator ShowLines()
    {
        isWriting = true;
        teclaContinue.SetActive(false);

        foreach (char ch in dialogueGroups[currentGroupIndex].dialogueLines[lineIndex])
        {
            dialogueTextInteract.text += ch;

            if (typeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typeSound);
            }

            yield return new WaitForSeconds(typingTime);
        }

        isWriting = false;

        isPlayerRange = true;


        if (!isWriting)
        {
            teclaContinue.SetActive(true);
        }

        if (dialoguePanelnteract == true)
        {
            playerMovement.canMove = false;
        }

        typingCoroutine = null;
    }

    private void EndDialogue()
    {
        isDialogueInProgress = false;
        dialoguePanelnteract.SetActive(false);

        teclaContinue.SetActive(false);
        isPlayerRange = false;
        boxCollider.enabled = true;

        if (playerMovement != null)
        {
            playerMovement.canMove = true;  // Llama a la función CanMove de PlayerMovement
        }

        if (playerInteraction != null)
        {
            playerInteraction.enabled = true;
        }


        if (llave != null) // Añade esta condición para verificar si el objeto llave existe
        {
            llave.SetActive(true);
        }

        if (currentGroupIndex < dialogueGroups.Length - 1)
        {
            currentGroupIndex++;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerRange = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerRange = false;
            EndDialogue();
        }
    }
}
