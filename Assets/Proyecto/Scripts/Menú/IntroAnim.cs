using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{
    [SerializeField] private float delayTime;
    public GameObject virtualCamera;
    public GameObject playerAnim;
    public GameObject tutorialCollider;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        StartCoroutine(FinishAnim());
        playerMovement.enabled = false;
        tutorialCollider.SetActive(false);

        virtualCamera.SetActive(false);
    }
    private IEnumerator FinishAnim()
    {
        yield return new WaitForSeconds(delayTime);
        playerMovement.enabled = true;
        playerAnim.SetActive(false);
        virtualCamera.SetActive(true);
        tutorialCollider.SetActive(true);
    }
}
