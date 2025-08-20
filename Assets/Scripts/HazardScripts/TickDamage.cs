using UnityEngine;

public class TickDamage : MonoBehaviour
{
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float damageInterval;
    [SerializeField] private HazardType hazardType;
    [SerializeField] private bool dealDamageInstantly;

    public void ToggleDealDamageInstantly()
    {
        dealDamageInstantly = !dealDamageInstantly;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHazardManager.Instance.EnterHazard(hazardType, minDamage, maxDamage, damageInterval, dealDamageInstantly);
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
