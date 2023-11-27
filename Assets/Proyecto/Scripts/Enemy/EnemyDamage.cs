using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerVida playerHealth = collision.gameObject.GetComponent<PlayerVida>();
            if (playerHealth != null)
            {
                playerHealth.RecibirDaño(damageAmount);
            }
        }
    }
}