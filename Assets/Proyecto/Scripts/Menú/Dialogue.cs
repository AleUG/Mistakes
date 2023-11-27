using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    private bool isPlayerRange = false;
    private bool isDialogueInProgress = false;
    private bool isWriting = false;

    public bool isActivate = false;

    [SerializeField] private GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;
    [SerializeField] private float timeDelay = 1.0f;

    private BoxCollider boxCollider;

    private int lineIndex;
    private Coroutine typingCoroutine;

    [SerializeField] float typingTime = 0.05f;

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

        //Encontrar los GameObjects
        dialoguePanel.SetActive(false);

        if (isActivate)
        {
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        if (isPlayerRange && !isDialogueInProgress)
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isPlayerRange = false;
        isDialogueInProgress = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = string.Empty;
        boxCollider.enabled = false;


        lineIndex = 0; // Iniciar con el primer elemento del grupo actual
        typingCoroutine = StartCoroutine(ShowLines());
    }

    private void NextDialogueLine()
    {
        if(lineIndex +1 >= dialogueGroups[currentGroupIndex].dialogueLines.Length)
        {
            EndDialogue();
            return ;
        }
        else 
        { 
            StartCoroutine(DelayTime());
        }
    }

    private IEnumerator ShowLines()
    {
        isWriting = true;
        
        for (int i = 0; i < dialogueGroups[currentGroupIndex].dialogueLines[lineIndex].Length; i++)
        {
            dialogueText.text += dialogueGroups[currentGroupIndex].dialogueLines[lineIndex][i];

            if (typeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(typeSound);
            }

            yield return new WaitForSeconds(typingTime);
        }

        isWriting = false;

        NextDialogueLine();
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
        StartCoroutine(DelayAndEndDialogue());
        dialoguePanel.SetActive(false);
        boxCollider.enabled = true;

        if (currentGroupIndex < dialogueGroups.Length - 1)
        {
            currentGroupIndex++;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player")  && !isDialogueInProgress)
        {
            isPlayerRange = true;
        }
    }
}
