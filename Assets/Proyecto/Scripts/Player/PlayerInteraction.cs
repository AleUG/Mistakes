using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // Rango de interacci�n.
    public LayerMask interactableLayer; // Capa de objetos interactuables.
    public Transform playerCamera; // La c�mara del jugador.
    public Material interactableMaterial; // Material Shader Graph interactuable.

    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    private void Update()
    {
        // Lanzar un rayo desde la c�mara del jugador en la direcci�n en la que est� mirando.
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactionRange, interactableLayer))
        {
            // El rayo golpe� un objeto interactuable.
            Interactable interactableObject = hit.collider.GetComponent<Interactable>();

            if (interactableObject != null)
            {
                Renderer[] renderers = hit.collider.GetComponentsInChildren<Renderer>();

                // Agrega un material adicional a los objetos hijos.
                foreach (Renderer renderer in renderers)
                {
                    if (!originalMaterials.ContainsKey(renderer))
                    {
                        // Almacena los materiales originales del objeto hijo si a�n no lo hemos hecho.
                        originalMaterials.Add(renderer, renderer.materials);
                    }

                    Material[] materials = renderer.materials;

                    // Asegura que haya suficientes elementos en el array de materiales para el material adicional.
                    if (materials.Length < 2)
                    {
                        Array.Resize(ref materials, 2);
                    }

                    Material clonedMaterial = new Material(interactableMaterial);
                    clonedMaterial.SetFloat("_Scale", 1.03f); // Modifica la propiedad del material clonado.

                    materials[1] = clonedMaterial; // Asigna el material adicional como el segundo material.

                    renderer.materials = materials; // Asigna el array de materiales actualizado al objeto hijo.
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // El jugador presion� la tecla "E", realiza la acci�n de interacci�n.
                    interactableObject.Interact();
                }
            }
        }
        else
        {
            // Restablece los materiales originales cuando el rayo no golpea ning�n objeto interactuable.
            foreach (var entry in originalMaterials)
            {
                Renderer renderer = entry.Key;
                renderer.materials = entry.Value; // Restablece los materiales originales.
            }

            originalMaterials.Clear(); // Limpia el diccionario.
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a line representing the interaction range in the Editor.
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + playerCamera.forward * interactionRange);
    }
}
