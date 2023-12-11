using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Linterna : MonoBehaviour
{
    private KeyCode linternKey = KeyCode.F;
    private GameObject linternaLight;
    private GameObject linternaObject;
    private AudioSource linternaAudio;

    private float batteryCharge = 1.0f; // Carga de la batería inicial
    public float batteryDrainRate = 0.05f; // Tasa de agotamiento de la batería por segundo

    private Image batteryImage;
    public bool isOn;

    private Inventario inventario;

    // Valor de desesperación
    private float desesperacion = 0.0f;
    public float aumentoDesesperacionPorSegundo = 0.05f;
    public float reduccionDesesperacionPorSegundo = 0.25f; // Nueva tasa de reducción de desesperación
    public float umbralDesesperacion = 0.8f; // Punto de desesperación crítica

    public float aumentoVelocidadLatido = 0.1f; // Ajusta este valor según sea necesario
    private AudioSource audioLatidoCorazon;

    //SpawnEnemy
    public List<Collider> colliders;
    private bool enemigoAparecido = false;
    public GameObject enemigoPrefab;
    public float probabilidadAparicionEnemigo = 0.75f;

    public bool isUnlock;
    public bool canDrainBattery = true;


    // Start is called before the first frame update
    void Start()
    {
        linternaLight = GameObject.Find("LinternaLight");
        linternaObject = GameObject.Find("LinternaObject");

        linternaLight.SetActive(false);
        linternaObject.SetActive(false);

        linternaAudio = GameObject.Find("LinternaAudio").GetComponent<AudioSource>();

        batteryImage = GameObject.Find("BatteryCharge").GetComponent<Image>();

        inventario = FindObjectOfType<Inventario>();

        audioLatidoCorazon = GameObject.Find("HeartBeat").GetComponent<AudioSource>();

        Lock();

        if (isUnlock)
        {
            Unlock();
        }
    }

    void Update()
    {
        LinternaVoid();

        Animator animator = linternaLight.GetComponent<Animator>();

        if (batteryCharge <= 0.1f)
        {
            animator.SetTrigger("LowBattery");
        }

        if (batteryCharge <= 0.0f)
        {
            // Si la batería está agotada, apaga la linterna
            isOn = false;
            animator.SetBool("IsBatteryOn", false);
            animator.SetTrigger("LuzTenue");
        }
        else
        {
            animator.SetBool("IsBatteryOn", true);
            if (linternaLight.activeSelf)
            {
                isOn = true;
            }
            else
            {
                isOn = false;
            }
        }

        // Verifica si la desesperación está por debajo del 80%
        if (desesperacion < umbralDesesperacion)
        {
            EliminarAlucinaciones();
        }
    }


    private void LinternaVoid()
    {
        if (Input.GetKeyDown(linternKey) && batteryCharge >= 0)
        {
            // Cambia el estado de la linterna al presionar "F"
            linternaLight.SetActive(!linternaLight.activeSelf);
            linternaAudio.Play();

            if (linternaLight.activeSelf)
            {
                isOn = true;

                // Añade la línea para reducir la desesperación mientras la linterna está encendida
                desesperacion -= reduccionDesesperacionPorSegundo * Time.deltaTime;
                desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperación esté en el rango [0, 1]
            }
            else
            {
                isOn = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && inventario.pilasActuales > 0)
        {
            inventario.GastarPila();
            RechargeBatery();
        }

        if (isOn)
        {
            if(canDrainBattery)// Reduce la carga de la batería con el tiempo
            {
                batteryCharge -= batteryDrainRate * Time.deltaTime;
                batteryCharge = Mathf.Clamp01(batteryCharge); // Asegura que la carga esté en el rango [0, 1]

                // Añade la línea para reducir la desesperación mientras la linterna está encendida
                desesperacion -= reduccionDesesperacionPorSegundo * Time.deltaTime;
                desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperación esté en el rango [0, 1]

                // Actualiza la imagen de la batería en el Canvas
                batteryImage.fillAmount = batteryCharge;

                // Ajusta la velocidad del latido del corazón gradualmente cuando la linterna está encendida
                float nuevaVelocidad = audioLatidoCorazon.pitch - Time.deltaTime * reduccionDesesperacionPorSegundo;
                audioLatidoCorazon.pitch = Mathf.Clamp(nuevaVelocidad, 1.0f, 2.0f); // Ahora el pitch no excederá 2.0
            }
        }
        else
        {
            // Incrementa la desesperación mientras la linterna está apagada
            desesperacion += aumentoDesesperacionPorSegundo * Time.deltaTime;
            desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperación esté en el rango [0, 1]


            // Ajusta la velocidad del latido del corazón según la desesperación
            float nuevaVelocidad = 1.0f + aumentoVelocidadLatido * desesperacion;
            audioLatidoCorazon.pitch = Mathf.Clamp(nuevaVelocidad, 1.0f, 2.0f); // Ahora el pitch no excederá 2.0

            // Verifica si la desesperación supera el umbral crítico
            if (!enemigoAparecido && desesperacion >= umbralDesesperacion)
            {
                // Imprime un mensaje de desesperación crítica en la consola
                Debug.Log("¡Estás desesperado! ¡Enciende la linterna para sentirte más seguro!");

                // Decide si el enemigo debe aparecer
                float randomValue = Random.value;
                if (desesperacion == 1.0f || randomValue <= probabilidadAparicionEnemigo)
                {
                    // Aparece el enemigo
                    SpawnEnemigo();
                    enemigoAparecido = true;  // Marca que el enemigo ya ha aparecido
                }
            }
        }
    }

    public void RechargeBatery()
    {
        batteryCharge = 1.0f;
        batteryImage.fillAmount = batteryCharge;

    }

    void SpawnEnemigo()
    {
        if (colliders.Count > 0)
        {
            // Escoge un collider aleatorio de la lista
            Collider randomCollider = colliders[Random.Range(0, colliders.Count)];

            // Obtiene un punto aleatorio dentro del collider
            Vector3 spawnPosition = GetRandomPointInCollider(randomCollider);

            // Instancia el enemigo en la posición especificada
            Instantiate(enemigoPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            // Si no hay colliders en la lista, utiliza una posición aleatoria como respaldo
            Vector3 spawnPosition = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
            Instantiate(enemigoPrefab, spawnPosition, Quaternion.identity);
        }

        Vector3 GetRandomPointInCollider(Collider collider)
        {
            // Obtiene un punto aleatorio dentro del collider usando las dimensiones del collider
            Vector3 randomPoint = new Vector3(
                Random.Range(collider.bounds.min.x, collider.bounds.max.x),
                Random.Range(collider.bounds.min.y, collider.bounds.max.y),
                Random.Range(collider.bounds.min.z, collider.bounds.max.z)
            );

            return randomPoint;
        }
    }

    // Método para destruir todos los enemigos en la escena
    void EliminarAlucinaciones()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Alucinación"); // Asigna el tag correcto a tus enemigos

        foreach (GameObject enemigo in enemigos)
        {
            Destroy(enemigo);
        }

        enemigoAparecido = false;  // Restablece la variable para permitir futuras apariciones
    }

    // Función para bloquear la linterna
    public void Lock()
    {
        enabled = false;
    }

    // Función para desbloquear la linterna
    public void Unlock()
    {
        enabled = true;
        linternaObject.SetActive(true);
        LinternaVoid();
    }
}
