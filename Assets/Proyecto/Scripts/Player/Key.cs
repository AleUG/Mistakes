using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool hasKey = false;
    public Door associatedDoor; // Referencia directa a la puerta asociada

    // M�todo para manejar la interacci�n con la llave
    public void Interact()
    {
        hasKey = true;
        // Puedes agregar aqu� animaciones u otros efectos para indicar que la llave ha sido recogida.
        CollectKey();
    }

    // M�todo para notificar al inventario que la llave ha sido recogida
    private void CollectKey()
    {
        Inventario inventario = FindObjectOfType<Inventario>();
        if (inventario != null)
        {
            inventario.RecogerLlave(this); // Informa al inventario que la llave ha sido recogida
        }

        if (associatedDoor != null)
        {
            associatedDoor.SetKey(this); // Informa a la puerta asociada sobre la llave correspondiente
        }
    }
}
