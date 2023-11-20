using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Linterna : MonoBehaviour
{
    private KeyCode linternKey = KeyCode.F;
    private GameObject linternaObject;
    private AudioSource linternaAudio;

    private float batteryCharge = 1.0f; // Carga de la bater�a inicial
    public float batteryDrainRate = 0.05f; // Tasa de agotamiento de la bater�a por segundo

    private Image batteryImage;
    private bool isOn;

    private Inventario inventario;

    // Valor de desesperaci�n
    private float desesperacion = 0.0f;
    public float aumentoDesesperacionPorSegundo = 0.1f;
    public float reduccionDesesperacionPorSegundo = 0.2f; // Nueva tasa de reducci�n de desesperaci�n
    public float umbralDesesperacion = 0.8f; // Punto de desesperaci�n cr�tica

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

                // A�ade la l�nea para reducir la desesperaci�n mientras la linterna est� encendida
                desesperacion -= reduccionDesesperacionPorSegundo * Time.deltaTime;
                desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperaci�n est� en el rango [0, 1]
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
            // Reduce la carga de la bater�a con el tiempo
            batteryCharge -= batteryDrainRate * Time.deltaTime;
            batteryCharge = Mathf.Clamp01(batteryCharge); // Asegura que la carga est� en el rango [0, 1]

            // A�ade la l�nea para reducir la desesperaci�n mientras la linterna est� encendida
            desesperacion -= reduccionDesesperacionPorSegundo * Time.deltaTime;
            desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperaci�n est� en el rango [0, 1]

            // Actualiza la imagen de la bater�a en el Canvas
            batteryImage.fillAmount = batteryCharge;

            if (batteryCharge <= 0.0f)
            {
                // Si la bater�a est� agotada, apaga la linterna
                isOn = false;
                linternaObject.SetActive(false);
            }
        }
        else
        {
            // Incrementa la desesperaci�n mientras la linterna est� apagada
            desesperacion += aumentoDesesperacionPorSegundo * Time.deltaTime;
            desesperacion = Mathf.Clamp01(desesperacion); // Asegura que la desesperaci�n est� en el rango [0, 1]

            // Verifica si la desesperaci�n supera el umbral cr�tico
            if (desesperacion >= umbralDesesperacion)
            {
                // Imprime un mensaje de desesperaci�n cr�tica en la consola
                Debug.Log("�Est�s desesperado! �Enciende la linterna para sentirte m�s seguro!");
            }
        }
    }

    public void RechargeBatery()
    {
        batteryCharge = 1.0f;
        batteryImage.fillAmount = batteryCharge;
    }
}
