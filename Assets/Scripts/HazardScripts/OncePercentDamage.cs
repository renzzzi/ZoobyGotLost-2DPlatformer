using UnityEngine;

public class OncePercentDamage : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float percentOfDamage;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered && !this.enabled) return;
   
        if (collision.gameObject.CompareTag("Player"))
        {
            float damageBasedOnHealth = PlayerStats.Instance.GetHealth() * percentOfDamage;
            int finalDamage = Mathf.RoundToInt(damageBasedOnHealth);

            PlayerStats.Instance.InflictDamage(finalDamage);
            hasTriggered = true;
            this.enabled = false;
        }
    }
}
