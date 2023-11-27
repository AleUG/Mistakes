using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlucination : MonoBehaviour
{
    //private GameObject canvasSusto;

    private EnemyAI enemyAI;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //canvasSusto.SetActive(true);

            enemyAI.chaseDistance = 15;
            StartCoroutine(ReturnChaseDistance());

        }
    }

    private IEnumerator ReturnChaseDistance()
    {
        yield return new WaitForSeconds(10);

        enemyAI.ResetChaseDistance();
        //canvasSusto.SetActive(false);
        Destroy(gameObject);
    }
}
