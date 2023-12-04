using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemy : MonoBehaviour
{
    private GameObject enemyActivate;

    // Start is called before the first frame update
    void Start()
    {
        enemyActivate = GameObject.Find("Enemy");
        StartCoroutine(DesactivateEnemy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyActivate.SetActive(true);
        }
    }

    private IEnumerator DesactivateEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        enemyActivate.SetActive(false);
    }
}
