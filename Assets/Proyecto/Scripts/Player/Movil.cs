using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movil : MonoBehaviour
{
    private KeyCode inputMóvil = KeyCode.Space;
    private GameObject movil;
    public GameObject colliderMovil;

    private AudioSource ringPhoneSound;
    private bool isCalling;

    public bool isLocked;

    // Start is called before the first frame update
    private void Start()
    {
        movil = GameObject.Find("Móvil");
        ringPhoneSound = GameObject.Find("phoneTone").GetComponent<AudioSource>();

        movil.SetActive(false);
        colliderMovil.SetActive(false);

        if (isLocked)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(inputMóvil) && !isCalling)
        {
            movil.SetActive(true);
            ringPhoneSound.Play();
            StartCoroutine(ActivarCollider());
            isCalling = true;
        }

        if (colliderMovil == false)
        {
            movil.SetActive(false);
        }
    }

    private IEnumerator ActivarCollider()
    {
        yield return new WaitForSeconds(5f);
        ringPhoneSound.Stop();
        colliderMovil.SetActive(true);
        isCalling = false;
    }

    public void Unlock()
    {
        enabled = true;
    }
}

