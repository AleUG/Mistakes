using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinternaScript : MonoBehaviour
{
    private KeyCode linternKey = KeyCode.F;
    private GameObject linternaObject;
    private AudioSource linternaAudio;

    // Start is called before the first frame update
    void Start()
    {
        linternaObject = GameObject.Find("Linterna");
        linternaObject.SetActive(false);

        linternaAudio = GameObject.Find("LinternaAudio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(linternKey))
        {
            // Cambia el estado de la linterna al presionar "F"
            linternaObject.SetActive(!linternaObject.activeSelf);
            linternaAudio.Play();
        }
    }
}
