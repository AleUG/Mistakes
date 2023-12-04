using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private CambiarEscena cambiarEscena;
    [SerializeField] private GameObject checkPointImage;
    public string chekcpointLevel;

    // Start is called before the first frame update
    void Start()
    {
        cambiarEscena = FindObjectOfType<CambiarEscena>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cambiarEscena.nombresEscenas[0] = chekcpointLevel;
            cambiarEscena.nombresEscenas[3] = chekcpointLevel;
            checkPointImage.SetActive(true);
            StartCoroutine(DesactivarImageCheckpoint());
        }
    }

    private IEnumerator DesactivarImageCheckpoint()
    {
        yield return new WaitForSeconds(1.5f);

        checkPointImage.SetActive(false);
        Destroy(gameObject);
    }
}
