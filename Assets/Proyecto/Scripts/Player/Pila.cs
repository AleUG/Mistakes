using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pila : MonoBehaviour
{
    private Linterna linterna;

    // Start is called before the first frame update
    void Start()
    {
        linterna = FindObjectOfType<Linterna>();
    }

    private void OnTriggerEnter()
    {
        linterna.RechargeBatery();
        Destroy(gameObject);
    }
}
