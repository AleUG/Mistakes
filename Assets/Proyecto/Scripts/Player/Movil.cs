using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movil : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
       enabled = true;
    }
}
