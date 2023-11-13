using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // Rango de interacción.
    public LayerMask interactableLayer; // Capa de objetos interactuables.
    public Transform playerCamera; // La cámara del jugador.
    public Material interactableMaterial; // Material Shader Graph interactuable.
    private GameObject textInteract;

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    private Renderer lastInteractedRenderer;

    private void Start()
    {

        textInteract = GameObject.Find("TextInteract");
        textInteract.SetActive(false);
    }

    private void Update()
    {
        // Lanzar un rayo desde la cámara del jugador en la dirección en la que está mirando.
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactionRange, interactableLayer))
        {
            textInteract.SetActive(true);

            if (lastInteractedRenderer != null)
            {
                // Restablece los materiales del último objeto interactuable.
                RestoreOriginalMaterials(lastInteractedRenderer);
                lastInteractedRenderer = null;
            }

            // El rayo golpeó un objeto interactuable.
            Interactable interactableObject = hit.collider.GetComponent<Interactable>();

            if (interactableObject != null)
            {
                Renderer[] renderers = hit.collider.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in renderers)
                {
                    if (!originalMaterials.ContainsKey(renderer))
                    {
                        originalMaterials.Add(renderer, renderer.materials);
                    }

                    Material[] materials = renderer.materials;

                    // Asegura que haya suficientes elementos en el array de materiales.
                    Array.Resize(ref materials, materials.Length + 1);

                    Material clonedMaterial = new Material(interactableMaterial);
                    clonedMaterial.SetFloat("_Scale", 1.05f);

                    materials[materials.Length - 1] = clonedMaterial; // Agrega el material al final del array.

                    renderer.materials = materials;
                }

                lastInteractedRenderer = renderers[0];

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (interactableObject.CompareTag("Pila"))
                    {
                        interactableObject.InteractPila();
                    }
                    else if (interactableObject.CompareTag("Door"))
                    {
                        interactableObject.InteractDoor();
                    }
                    else
                    {
                        interactableObject.InteractArmario();
                    }
                }
            }
        }
        else if (lastInteractedRenderer != null)
        {
            // Restablece los materiales del último objeto interactuable cuando no se mira ninguno.
            RestoreOriginalMaterials(lastInteractedRenderer);
            lastInteractedRenderer = null;
            textInteract.SetActive(false);
        }
    }

    private void RestoreOriginalMaterials(Renderer renderer)
    {
        Material[] original = originalMaterials[renderer];
        renderer.materials = original;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + playerCamera.forward * interactionRange);
    }
}


