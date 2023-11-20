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
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;

    public GameObject llave;

    private BoxCollider boxCollider;

    private int lineIndex;
    private Coroutine typingCoroutine;

    private float typingTime = 0.05f;

    private PlayerMovement playerMovement;

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
                StopCoroutine(typingCoroutine);
                dialogueText.text = dialogueGroups[currentGroupIndex].dialogueLines[lineIndex];
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

        dialoguePanel.SetActive(true);
        dialogueText.text = string.Empty;
        boxCollider.enabled = false;

        if (playerMovement != null)
        {
            playerMovement.canMove = false;
        }

        lineIndex = 0; // Iniciar con el primer elemento del grupo actual

        typingCoroutine = StartCoroutine(ShowLines());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        dialogueText.text = string.Empty;
        typingCoroutine = StartCoroutine(ShowLines());

        if (dialoguePanel == true)
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
            dialogueText.text += ch;

            if (typeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typeSound);
            }

            yield return new WaitForSeconds(typingTime);
        }

        isWriting = false;

        if (playerMovement != null && lineIndex == dialogueGroups[currentGroupIndex].dialogueLines.Length - 1)
        {
            playerMovement.canMove = true;
        }

        isPlayerRange = true;


        if (!isWriting)
        {
            teclaContinue.SetActive(true);
        }

        if (dialoguePanel == true)
        {
            playerMovement.canMove = false;
        }

        typingCoroutine = null;
    }

    private void EndDialogue()
    {
        isDialogueInProgress = false;
        dialoguePanel.SetActive(false);

        teclaContinue.SetActive(false);
        isPlayerRange = false;
        boxCollider.enabled = true;

        if (playerMovement != null)
        {
            playerMovement.canMove = true;  // Llama a la función CanMove de PlayerMovement
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

        }
    }
}
