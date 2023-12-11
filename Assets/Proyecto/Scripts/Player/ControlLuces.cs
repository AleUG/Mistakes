using UnityEngine;

public class ControlLuces : MonoBehaviour
{
    public float distanciaActivacion = 10f; // Distancia a la que se activar�n las luces.

    private void Update()
    {
        // Obt�n todas las luces del entorno (puedes ajustar esto seg�n tu estructura de escena).
        GameObject[] lucesEntorno = GameObject.FindGameObjectsWithTag("Luz");

        // Encuentra la posici�n actual del jugador.
        Vector3 posicionJugador = transform.position;

        foreach (GameObject luz in lucesEntorno)
        {
            // Calcula la distancia entre el jugador y la luz.
            float distancia = Vector3.Distance(posicionJugador, luz.transform.position);

            // Activa o desactiva la luz seg�n la distancia.
            if (distancia <= distanciaActivacion)
            {
                luz.GetComponent<Light>().enabled = true;
            }
            else
            {
                luz.GetComponent<Light>().enabled = false;
            }
        }
    }
}
