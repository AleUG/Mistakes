using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemysLinterna : MonoBehaviour
{
    private Linterna linterna;


    // Start is called before the first frame update
    void Start()
    {
        linterna = FindObjectOfType<Linterna>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            linterna.canDrainBattery = true;
        }

    }
}
