using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject keyDoor;
    private bool hasKey = false;

    public void Interact()
    {
        if (!hasKey)
        {
            // El jugador ha recogido la llave
            hasKey = true;

            // Desbloquear la puerta asociada a la llave
            if (keyDoor != null)
            {
                keyDoor.GetComponent<Door>().Unlock();
            }
        }
    }

}
