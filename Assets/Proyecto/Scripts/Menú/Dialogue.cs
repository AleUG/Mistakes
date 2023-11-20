using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    private bool isPlayerRange = false;
    private bool isDialogueInProgress = false;
    private bool isWriting = false;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;
    [SerializeField] private float timeDelay = 1.0f;

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
        if (isPlayerRange && !isDialogueInProgress)
        {
            StartDialogue();
        }

        if (isDialogueInProgress && isWriting)
        {
            if (lineIndex < dialogueGroups[currentGroupIndex].dialogueLines.Length - 1)
            {
                NextDialogueLine();
            }
            else
            {
                StartCoroutine(DelayAndEndDialogue());
            }
        }
    }

    private void StartDialogue()
    {
        isDialogueInProgress = true;

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
        StartCoroutine(DelayTime());
    }

    private IEnumerator ShowLines()
    {
        isWriting = true;

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

        if (!isWriting)
        {
            StartCoroutine(DelayAndEndDialogue());
        }
    }

    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(timeDelay);

        lineIndex++;
        dialogueText.text = string.Empty;
        typingCoroutine = StartCoroutine(ShowLines());
    }

    private IEnumerator DelayAndEndDialogue()
    {
        yield return new WaitForSeconds(timeDelay);

        EndDialogue();
    }

    private void EndDialogue()
    {
        isDialogueInProgress = false;
        dialoguePanel.SetActive(false);
        boxCollider.enabled = true;

        if (playerMovement != null)
        {
            playerMovement.canMove = true;
        }

        // Si deseas activar un objeto (por ejemplo, llave) al final del diálogo, descomenta las siguientes líneas:
        // if (llave != null)
        // {
        //     llave.SetActive(true);
        // }

        if (currentGroupIndex < dialogueGroups.Length - 1)
        {
            currentGroupIndex++;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDialogueInProgress)
        {
            isPlayerRange = true;
            StartDialogue();
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
