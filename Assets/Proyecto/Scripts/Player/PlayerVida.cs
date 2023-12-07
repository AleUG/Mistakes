using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerVida : MonoBehaviour
{
    [SerializeField] private float vidaMaxima; // Vida m�xima del jugador
    [SerializeField] private float vidaActual; // Vida actual del jugador
    public Image[] damageImages; // Referencia a las im�genes de da�o en el Inspector

    private bool invulnerable = false;
    private bool isTakingDamage = false; // Indica si el jugador est� recibiendo da�o actualmente

    private Rigidbody rb;
    public float invulnerabilityDuration = 2f;

    private int damageImageIndex = -1; // �ndice de la imagen de da�o actual
    private float damageImageStartTime = 0f; // Hora de inicio para el efecto de fade out
    private float damageImageDuration = 1f; // Duraci�n del efecto de fade out

    private GameObject enemyDesactivar;
    private AudioSource screamerAudioSource;
    private AudioSource music;

    public GameObject canvasGameOver;
    private GameObject desactivarGameObject;
    private Animator animator;
    private PlayerMovement playerMovement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        enemyDesactivar = GameObject.Find("Enemy");

        music = GameObject.Find("Music").GetComponent<AudioSource>();
        screamerAudioSource = GameObject.Find("ScreamerAudio").GetComponent<AudioSource>();
        desactivarGameObject = GameObject.Find("PauseManager");
    }

    public void RecibirDa�o(int cantidad)
    {
        if (invulnerable || isTakingDamage)
        {
            return;
        }

        vidaActual -= cantidad; // Resta la cantidad de da�o a la vida actual

        if (vidaActual <= 0)
        {
            playerMovement.canMove = false;
            enemyDesactivar.SetActive(false);
            animator.SetTrigger("Die");
            screamerAudioSource.Play();

            music.Stop();

            StartCoroutine(Die());

            SetVidaMaxima();
        }
        else
        {
            // Activa invulnerabilidad y el parpadeo del sprite
            ActivarInvulnerabilidad();
            Invoke("DesactivarInvulnerabilidad", invulnerabilityDuration);

            // Indicar que el jugador est� recibiendo da�o
            isTakingDamage = true;

            // Muestra una imagen de da�o aleatoria
            MostrarImagenDeDanoAleatoria();
        }
    }

    private void MostrarImagenDeDanoAleatoria()
    {
        if (damageImages.Length > 0)
        {
            // Desactiva todas las im�genes de da�o antes de mostrar una aleatoria
            foreach (Image damageImage in damageImages)
            {
                damageImage.gameObject.SetActive(false);
            }

            // Muestra la siguiente imagen de da�o aleatoria
            damageImageIndex = (damageImageIndex + 1) % damageImages.Length;
            damageImages[damageImageIndex].gameObject.SetActive(true);

            // Inicia el temporizador para el efecto de fade out
            damageImageStartTime = Time.time;
        }
    }

    public void ActivarInvulnerabilidad()
    {
        invulnerable = true;
    }

    public void DesactivarInvulnerabilidad()
    {
        invulnerable = false;
        isTakingDamage = false;
    }

    public void Curar(int cantidad)
    {
        vidaActual += cantidad; // Suma la cantidad de curaci�n a la vida actual

        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima; // Limita la vida actual al valor m�ximo
        }
    }

    public void SetVidaMaxima()
    {
        vidaActual = vidaMaxima;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1.0f);

        canvasGameOver.SetActive(true);
        desactivarGameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;


    }

    void Update()
    {
        // Comprueba si es necesario ocultar la imagen de da�o con un efecto de fade out
        if (damageImageIndex >= 0 && Time.time - damageImageStartTime >= damageImageDuration)
        {
            damageImages[damageImageIndex].gameObject.SetActive(false);
        }

    }
}