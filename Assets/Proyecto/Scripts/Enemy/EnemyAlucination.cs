using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlucination : MonoBehaviour
{
    private EnemyAI enemyAI;
    [SerializeField] private MeshRenderer[] meshDesactivate;

    void Start()
    {
        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si el enemigo no está persiguiendo, modifica chaseDistance y comienza el temporizador
            enemyAI.SetAlucinating(true); // Indica al enemigo que está alucinando

            // Desactiva cada MeshRenderer en el array
            foreach (MeshRenderer meshRenderer in meshDesactivate)
            {
                meshRenderer.enabled = false;
            }

            StartCoroutine(ReturnChaseDistance());
        }
    }


    private IEnumerator ReturnChaseDistance()
    {
        yield return new WaitForSeconds(5);

        enemyAI.SetAlucinating(false); // Indica al enemigo que ya no está alucinando

        Destroy(gameObject, 0.1f);
    }
}
