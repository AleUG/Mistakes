using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool hasKey = false;
    public Door associatedDoor; // Referencia directa a la puerta asociada

    public GameObject[] desactivarObjetos;

    // Método para manejar la interacción con la llave
    public void Interact()
    {
        hasKey = true;
        // Puedes agregar aquí animaciones u otros efectos para indicar que la llave ha sido recogida.
        CollectKey();

        if (desactivarObjetos != null)
        {
            foreach (GameObject obj in desactivarObjetos)
            {
                if (obj != null)
                {
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }
    }

    // Método para notificar al inventario que la llave ha sido recogida
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
