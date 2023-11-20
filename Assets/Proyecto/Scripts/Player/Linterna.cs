using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Linterna : MonoBehaviour
{
    private KeyCode linternKey = KeyCode.F;
    private GameObject linternaObject;
    private AudioSource linternaAudio;

    private float batteryCharge = 1.0f; // Carga de la batería inicial
    public float batteryDrainRate = 0.05f; // Tasa de agotamiento de la batería por segundo

    private Image batteryImage;
    private bool isOn;

    private Inventario inventario;

    // Valor de desesperación
    private float desesperacion = 0.0f;
    public float aumentoDesesperacionPorSegundo = 0.1f;
    public float reduccionDesesperacionPorSegundo = 0.2f; // Nueva tasa de reducción de desesperación
    public float umbralDesesperacion = 0.8f; // Punto de desesperación crítica

    // Start is called before the first frame update
    void Start()
    {
        linternaObject = GameObject.Find("LinternaLight");
        linternaObject.SetActive(false);

        linternaAudio = GameObject.Find("LinternaAudio").GetComponent<AudioSource>();

        batteryImage = GameObject.Find("BatteryCharge").GetComponent<Image>();

        inventario = FindObjectOfType<Inventario>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(linternKey) && batteryCharge >= 0)
        {
            // Cambia el estado de la linterna al presionar "F"
            linternaObject.SetActive(!linternaObject.activeSelf);
            linternaAudio.Play();

            if (linternaObject.activeSelf)
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
            // Reduce la carga de la batería con el tiempo
            batteryCharge -= batteryDrainRate * Time.deltaTime;
            batteryCharge = Mathf.Clamp01(batteryCharge); // Asegura que la carga esté en el rango [0, 1]

            // Añade la línea para reducir la desesperación mientras la linterna está encendida
            desesperacion -= reduccionDesesperacionPorSegundo * Time.deltaTime;
            desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperación esté en el rango [0, 1]

            // Actualiza la imagen de la batería en el Canvas
            batteryImage.fillAmount = batteryCharge;

            if (batteryCharge <= 0.0f)
            {
                // Si la batería está agotada, apaga la linterna
                isOn = false;
                linternaObject.SetActive(false);
            }
        }
        else
        {
            // Incrementa la desesperación mientras la linterna está apagada
            desesperacion += aumentoDesesperacionPorSegundo * Time.deltaTime;
            desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperación esté en el rango [0, 1]

            // Verifica si la desesperación supera el umbral crítico
            if (desesperacion >= umbralDesesperacion)
            {
                // Imprime un mensaje de desesperación crítica en la consola
                Debug.Log("¡Estás desesperado! ¡Enciende la linterna para sentirte más seguro!");
            }
        }
    }

    public void RechargeBatery()
    {
        batteryCharge = 1.0f;
        batteryImage.fillAmount = batteryCharge;
    }
}
