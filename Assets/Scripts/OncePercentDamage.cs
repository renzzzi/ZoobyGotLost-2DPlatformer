using UnityEngine;

public class OncePercentDamage : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] float percentOfDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float damageBasedOnHealth = PlayerStats.Instance.GetHealth() * percentOfDamage;
            int finalDamage = Mathf.RoundToInt(damageBasedOnHealth);

            PlayerStats.Instance.InflictDamage(finalDamage);
            this.enabled = false;
        }
    }
}
