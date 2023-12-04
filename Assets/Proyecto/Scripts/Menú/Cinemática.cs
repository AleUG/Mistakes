using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinemática : MonoBehaviour
{
    public string nextSceneName; // Nombre de la escena a cargar después de la cinemática
    public float cinematicDuration = 17.0f; // Duración de la cinemática en segundos

    private void Start()
    {
        // Reproducir la cinemática aquí

        // Esperar a que la cinemática termine
        Invoke("LoadNextScene", cinematicDuration);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        // Cargar la siguiente escena
        SceneManager.LoadScene(nextSceneName);
    }
}