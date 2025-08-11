using UnityEngine;
using System.Collections;

public class VentWindCreation : MonoBehaviour
{
    [SerializeField] private GameObject strongVentWindsPrefab;

    [Header("Strong Vent Winds Generation Settings")]
    [SerializeField] private float minIntervalDuration;
    [SerializeField] private float maxIntervalDuration;

    private Coroutine ventCoroutine;
    
    private void Awake()
    {
        ventCoroutine = StartCoroutine(GenerateStrongVentWinds());
    }

    private void OnDestroy()
    {
        StopCoroutine(ventCoroutine);
    }

    private IEnumerator GenerateStrongVentWinds()
    {
        if (strongVentWindsPrefab == null)
        {
            Debug.Log("Strong Vent Winds Prefab has not been assigned.");
            yield break;
        }

        while (gameObject.activeSelf)
        {
            // Instantiates the prefab and sets its movement, speed, and interval of each wind instantiation
            Instantiate(strongVentWindsPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(Random.Range(minIntervalDuration, maxIntervalDuration));
        }
    }
}
 