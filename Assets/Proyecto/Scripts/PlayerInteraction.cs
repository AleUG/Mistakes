using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // Rango de interacción.
    public LayerMask interactableLayer; // Capa de objetos interactuables.
    public Transform playerCamera; // La cámara del jugador.

    private void Update()
    {
        // Lanzar un rayo desde la cámara del jugador en la dirección en la que está mirando.
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactionRange, interactableLayer))
        {
            // El rayo golpeó un objeto interactuable.
            Interactable interactableObject = hit.collider.GetComponent<Interactable>();

            if (interactableObject != null)
            {
                // Mostrar algún tipo de indicador visual, como cambiar el color del objeto o mostrar un mensaje al jugador.

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // El jugador presionó la tecla "E", realiza la acción de interacción.
                    interactableObject.Interact();
                }
            }
        }
        else
        {
            // El rayo no golpeó ningún objeto interactuable, por lo que puedes ocultar el indicador visual si lo mostraste previamente.
        }
    }
}
