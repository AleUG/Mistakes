using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneCollider : MonoBehaviour
{
    // Nombre de la escena a la que deseas cambiar.
    public int indexEscena;
    [SerializeField] private CambiarEscena cambiarEscena;

    private void Start()
    {
        cambiarEscena.gameObject.GetComponent<CambiarEscena>();
    }

    // Método para cambiar de escena cuando se activa el trigger.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cambiarEscena.CambiarASiguienteEscena(indexEscena);

            //SceneManager.LoadScene(nombreDeEscena);
        }
    }
}