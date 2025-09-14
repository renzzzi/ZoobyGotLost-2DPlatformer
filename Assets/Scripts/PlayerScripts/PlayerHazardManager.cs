using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHazardManager : MonoBehaviour
{
    public static PlayerHazardManager Instance {  get; private set; }
   
    private readonly Dictionary<HazardType, int> activeHazardCount = new();
    private readonly Dictionary<HazardType, Coroutine> activeHazardCoroutines = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnterHazard(HazardType hazardType, int minDamage, int maxDamage, float damageInterval, bool dealDamageInstantly)
    {
        if (PlayerStats.Instance.GetIsDead()) return;

        if (!activeHazardCount.ContainsKey(hazardType))
        {
            activeHazardCount[hazardType] = 0;
        }

        activeHazardCount[hazardType]++;

        if (activeHazardCount[hazardType] == 1)
        {
            activeHazardCoroutines[hazardType] = StartCoroutine(DealDamage(minDamage, maxDamage, damageInterval, dealDamageInstantly));
        }
    }

    public void ExitHazard(HazardType hazardType)
    {
        if (PlayerStats.Instance.GetIsDead()) return;

        if (activeHazardCount.ContainsKey(hazardType))
        {
            activeHazardCount[hazardType]--;

            if (activeHazardCount[hazardType] <= 0)
            {
                if (activeHazardCoroutines.ContainsKey(hazardType) && activeHazardCoroutines[hazardType] != null)
                {
                    StopCoroutine(activeHazardCoroutines[hazardType]);
                    activeHazardCoroutines[hazardType] = null;
                }
            }
        }
    }


    private IEnumerator DealDamage(int minDamage, int maxDamage, float damageInterval, bool dealDamageInstantly)
    {
        if (!dealDamageInstantly)
        {
            yield return new WaitForSeconds(damageInterval * 0.4f);
        }

        while (!PlayerStats.Instance.GetIsDead())
        {
            int randomDamage = Random.Range(minDamage, maxDamage + 1);
            PlayerStats.Instance.InflictDamage(randomDamage);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
