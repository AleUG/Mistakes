using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Interactable : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public MeshRenderer player;

    private PlayerMovement playerMovement;
    private bool isOpen;

    private Animator animator;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        animator = GetComponent<Animator>();

    }

    public void Interact()
    {
        Debug.Log("interact with" , this.gameObject);

        if(isOpen)
        {
            animator.SetTrigger("Close");
            isOpen = false;
            virtualCamera.gameObject.SetActive(false);
            playerMovement.enabled = true;
            player.enabled = false;
        }
        else
        {
            animator.SetTrigger("Open");
            isOpen = true;
            virtualCamera.gameObject.SetActive(true);
            playerMovement.enabled = false;
            player.enabled = true;
        }


    }
}
