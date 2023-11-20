using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = true;
    private Key currentKey; // Llave actual asociada a la puerta

    private void Update()
    {
        // Verifica si el jugador est� interactuando con la puerta (por ejemplo, presionando la tecla E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Llama al m�todo Unlock solo si la puerta est� bloqueada y la llave pertenece a esta puerta
            if (isLocked && CheckKey())
            {
                Unlock();
            }
        }
    }

    private bool CheckKey()
    {
        // Verifica si la llave actual existe y est� asociada a esta puerta
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
        // Puedes agregar aqu� animaciones u otros efectos para indicar que la puerta est� desbloqueada.
        Debug.Log("La puerta ha sido desbloqueada.");
    }
}
