using UnityEngine;

public class TickDamage : MonoBehaviour
{
    [SerializeField] private int minDamage = 5;
    [SerializeField] private int maxDamage = 9;
    [SerializeField] private float damageInterval = 1.0f;
    [SerializeField] private PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStats.EnterHazard(minDamage, maxDamage, damageInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStats.ExitHazard();
        }
    }

}
