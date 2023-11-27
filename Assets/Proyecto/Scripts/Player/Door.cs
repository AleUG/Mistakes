using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = true;
    private Key currentKey; // Llave actual asociada a la puerta

    private void Update()
    {
        // Verifica si el jugador está interactuando con la puerta (por ejemplo, presionando la tecla E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isLocked && CheckKey())
            {
                Unlock();
            }
        }
    }

    private bool CheckKey()
    {
        // Verifica si la llave actual existe y está asociada a esta puerta
        return currentKey != null && currentKey.hasKey && currentKey.associatedDoor == this;
    }

    public void SetKey(Key key)
    {
        // Establece la llave asociada a la puerta
        currentKey = key;
    }

    public void Unlock()
    {
        isLocked = false;

    }
}
