using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isLocked = true;

    public void Unlock()
    {
        isLocked = false;
        // Puedes agregar aquí animaciones u otros efectos para indicar que la puerta está desbloqueada.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {
            // El jugador puede atravesar la puerta porque está desbloqueada
            // Puedes agregar aquí animaciones u otros efectos para abrir la puerta.
            Debug.Log("Puerta abierta");
        }
    }
}