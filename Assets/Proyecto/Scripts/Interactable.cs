using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Color highlightColor = Color.yellow; // Color de resaltado al interactuar.
    private Color originalColor; // Almacena el color original del objeto.

    private void Start()
    {
        // Almacenar el color original del objeto.
        originalColor = GetComponent<Renderer>().material.color;
    }

    public void Interact()
    {
        // Aquí puedes definir la acción de interacción que deseas realizar.
        // Por ejemplo, cambiar el color del objeto al color de resaltado.
        GetComponent<Renderer>().material.color = highlightColor;

        // Puedes agregar más lógica de interacción aquí, como abrir una puerta, recoger un objeto, etc.

        // Para restablecer el color original después de un tiempo, puedes usar una corutina o un temporizador.
    }
}
