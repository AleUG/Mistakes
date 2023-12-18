using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SensibilidadMouse : MonoBehaviour
{
    public Slider sensibilidadSlider; // Referencia al slider en el canvas
    public float sensibilidad;
    public float sensibilidadMax = 400.0f;
    public float sensibilidadMin = 100.0f;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV pov;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();

        // Cargar la sensibilidad desde PlayerPrefs
        sensibilidad = PlayerPrefs.GetFloat("Sensibilidad", 2.0f);

        // Configurar el slider
        sensibilidadSlider.minValue = sensibilidadMin;
        sensibilidadSlider.maxValue = sensibilidadMax;
        sensibilidadSlider.value = sensibilidad;
        sensibilidadSlider.onValueChanged.AddListener(ChangeSensibilidad);
    }

    void Update()
    {
        // Obtener el movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Modificar la sensibilidad
        mouseX *= sensibilidad;
        mouseY *= sensibilidad;

        // Ajustar la sensibilidad del movimiento de la cámara
       
        pov.m_HorizontalAxis.m_MaxSpeed = sensibilidad;
        pov.m_VerticalAxis.m_MaxSpeed = sensibilidad;
    }

    // Método llamado cuando se cambia el valor del slider
    void ChangeSensibilidad(float value)
    {
        sensibilidad = value;

        // Guardar la sensibilidad en PlayerPrefs
        PlayerPrefs.SetFloat("Sensibilidad", sensibilidad);
    }
}
