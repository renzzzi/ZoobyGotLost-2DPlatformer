using UnityEngine;
using System.Collections;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float damageInterval;
    [Tooltip("The duration of taking damage even after leaving the hazard.")]
    [SerializeField] private float carryOnDamageDuration;

    [SerializeField] private HazardType hazardType;
    [SerializeField] private bool dealDamageInstantly;
    private bool inHazard = false;
    private Coroutine DOTCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inHazard = true;
            PlayerHazardManager.Instance.EnterHazard(hazardType, minDamage, maxDamage, damageInterval, dealDamageInstantly);
            DOTCoroutine = StartCoroutine(DOT());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inHazard = false;
        }
    }

    private IEnumerator DOT()
    {
        while (inHazard)
        {
            yield return null;
        }
        yield return new WaitForSeconds(carryOnDamageDuration);
        inHazard = false;
        PlayerHazardManager.Instance.ExitHazard(hazardType);
        DOTCoroutine = null;
    }
}
