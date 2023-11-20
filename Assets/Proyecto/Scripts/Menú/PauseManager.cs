using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas; // Referencia al canvas de pausa
    public GameObject optionsCanvas;
    public bool isPaused = false; // Indica si el juego está pausado

    private void Start()
    {
        ResumeGame(); // Inicia el juego sin pausa
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Si el juego ya está pausado, resumirlo
            }
            else
            {
                PauseGame(); // Si el juego no está pausado, pausarlo
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Establecer el timescale en 0 para pausar el juego
        isPaused = true;
        pauseCanvas.SetActive(true); // Activar el canvas de pausa
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Restaurar el timescale a 1 para reanudar el juego
        isPaused = false;
        pauseCanvas.SetActive(false); // Desactivar el canvas de pausa
        optionsCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}