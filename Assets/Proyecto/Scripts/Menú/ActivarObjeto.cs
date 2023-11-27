using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActivarObjeto : MonoBehaviour
{
    public List<GameObject> canvasObjects; // Lista de objetos Canvas a controlar
    public GameObject colliderActivate;
    public float fadeInDuration = 1f; // Duración de la transición de opacidad (fade-in)
    public float fadeOutDuration = 1f; // Duración de la transición de opacidad (fade-out)

    private bool enContacto;
    private float currentOpacity = 0f;
    private float fadeSpeed;

    private void Start()
    {
        // Desactiva los objetos Canvas al inicio
        foreach (var canvasObject in canvasObjects)
        {
            canvasObject.SetActive(false);
        }
        fadeSpeed = 1f / fadeInDuration;
    }

    private void Update()
    {
        float targetOpacity = enContacto ? 1f : 0f;
        currentOpacity = Mathf.MoveTowards(currentOpacity, targetOpacity, fadeSpeed * Time.deltaTime);

        foreach (var canvasObject in canvasObjects)
        {
            // Asegura que el objeto Canvas tenga un componente Graphic (como Image o Text)
            Graphic graphic = canvasObject.GetComponent<Graphic>();

            if (graphic != null)
            {
                // Actualiza la opacidad del componente Graphic
                Color color = graphic.color;
                color.a = currentOpacity;
                graphic.color = color;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Cambia "Player" por la etiqueta que desees
        {
            enContacto = true;
            currentOpacity = 0f; // Reiniciar la opacidad
            foreach (var canvasObject in canvasObjects)
            {
                canvasObject.SetActive(true); // Activa los objetos Canvas al entrar en contacto
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Animator animator = colliderActivate.GetComponent<Animator>();

        if (other.CompareTag("Player")) // Cambia "Player" por la etiqueta que desees
        {
            enContacto = false;
            Destroy(gameObject, 1.0f);
            animator.SetTrigger("Close");
        }
    }
}
