using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventario : MonoBehaviour
{
    public int pilasActuales = 1;
    public int pilasMáximas = 3;

    private TextMeshProUGUI pilasText; // Un Text en el Canvas para mostrar la cantidad de pilas

    private void Start()
    {
        pilasText = GameObject.Find("PilasText").GetComponent<TextMeshProUGUI>();
        ActualizarUI(); // Actualizar la UI al inicio del juego
    }

    // Método para recoger pilas
    public void RecogerPila(int cantidad)
    {
        pilasActuales += cantidad;
        pilasActuales = Mathf.Clamp(pilasActuales, 0, pilasMáximas); // Asegurarse de que no exceda el límite máximo
        ActualizarUI();
    }

    // Método para gastar pilas
    public bool GastarPila()
    {
        if (pilasActuales > 0)
        {
            pilasActuales--;
            ActualizarUI();
            return true;
        }
        else
        {
            return false; // No hay pilas para gastar
        }
    }

    // Método para actualizar la UI
    private void ActualizarUI()
    {
        pilasText.text = pilasActuales + "/" + pilasMáximas;
    }
}
