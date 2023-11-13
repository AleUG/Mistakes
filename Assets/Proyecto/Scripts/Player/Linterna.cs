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

            // Actualiza la imagen de la batería en el Canvas
            batteryImage.fillAmount = batteryCharge;

            if (batteryCharge <= 0.0f)
            {
                // Si la batería está agotada, apaga la linterna
                isOn = false;
                linternaObject.SetActive(false);
            }
        }
    }

    public void RechargeBatery()
    {
        batteryCharge = 1.0f;
        batteryImage.fillAmount = batteryCharge;
    }
}
