using UnityEngine;

public class HealingOrbController : MonoBehaviour
{
    [SerializeField] int healAmount;
    bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isUsed) return;
            int maxHealth = 100;
            int currentHealth = PlayerStats.Instance.GetHealth();
            int finalHealAmount = healAmount;
            int potentialHealth = currentHealth + finalHealAmount;

            if (potentialHealth > 100)
            {
                finalHealAmount = maxHealth - currentHealth;
            }
            PlayerStats.Instance.AddHealth(finalHealAmount);
            isUsed = true;
            Destroy(gameObject);
        }
    }
}
