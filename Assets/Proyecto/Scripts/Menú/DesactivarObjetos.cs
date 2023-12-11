using System.Collections;
using UnityEngine;

public class DesactivarObjetos : MonoBehaviour
{
    public GameObject[] gameObjects;
    public float timeDelay;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DesactivarGameObjects();
            StartCoroutine(EndAnimation());
        }
    }

    private void DesactivarGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    private void ActivarGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(!obj.activeSelf);
        }
    }

    private IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(timeDelay);

        ActivarGameObjects();
    }
}
