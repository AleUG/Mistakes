using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // Rango de interacci�n.
    public LayerMask interactableLayer; // Capa de objetos interactuables.
    public Transform playerCamera; // La c�mara del jugador.

    private void Update()
    {
        // Lanzar un rayo desde la c�mara del jugador en la direcci�n en la que est� mirando.
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactionRange, interactableLayer))
        {
            // El rayo golpe� un objeto interactuable.
            Interactable interactableObject = hit.collider.GetComponent<Interactable>();

            if (interactableObject != null)
            {
                // Mostrar alg�n tipo de indicador visual, como cambiar el color del objeto o mostrar un mensaje al jugador.

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // El jugador presion� la tecla "E", realiza la acci�n de interacci�n.
                    interactableObject.Interact();
                }
            }
        }
        else
        {
            // El rayo no golpe� ning�n objeto interactuable, por lo que puedes ocultar el indicador visual si lo mostraste previamente.
        }
    }
}
