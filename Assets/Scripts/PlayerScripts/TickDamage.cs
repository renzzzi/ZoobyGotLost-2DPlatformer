using UnityEngine;

public class TickDamage : MonoBehaviour
{
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float damageInterval;
    [SerializeField] private HazardType hazardType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHazardManager.Instance.EnterHazard(hazardType, minDamage, maxDamage, damageInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHazardManager.Instance.ExitHazard(hazardType);
        }
    }
}
