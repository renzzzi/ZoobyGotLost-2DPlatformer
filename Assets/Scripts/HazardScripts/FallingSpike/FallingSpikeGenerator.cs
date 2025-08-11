using UnityEngine;
using System.Collections;

// Attach to FallingSpikeBlock
public class FallingSpikeGenerator : MonoBehaviour
{
    [SerializeField] GameObject fallingSpikePrefab;
    [SerializeField] float warningDuration;

    [Tooltip("The speed in which the falling spikes generate")]
    [SerializeField] float minIntervalDuration;
    [SerializeField] float maxIntervalDuration;
    private Coroutine generateFallingSpikesCoroutine;

    private void Awake()
    {
        if (generateFallingSpikesCoroutine == null)
        {
            generateFallingSpikesCoroutine = StartCoroutine(GenerateFallingSpike());
        }
    }

    private void OnDestroy()
    {
        if (generateFallingSpikesCoroutine != null)
        {
            StopCoroutine(generateFallingSpikesCoroutine);
        }
    }

    private IEnumerator GenerateFallingSpike()
    {
        if (fallingSpikePrefab == null)
        {
            Debug.Log("Falling spike prefab missing!");
            yield break;
        }

        Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);

        while (gameObject.activeSelf)
        {
            float randomNumber = Random.Range(minIntervalDuration, maxIntervalDuration);
            yield return new WaitForSeconds(randomNumber);

            GameObject newFallingSpike = Instantiate(fallingSpikePrefab, spawnLocation, transform.rotation);
            yield return new WaitForSeconds(warningDuration);
            newFallingSpike.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            newFallingSpike.GetComponent<Animator>().SetTrigger("Out");
        }
    }
}
