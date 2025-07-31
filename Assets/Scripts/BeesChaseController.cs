using System.Collections;
using UnityEngine;

public class BeesChaseController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform beesTransform;
    [SerializeField] private GameObject bees;
    [SerializeField] private float beesMovementSpeed;

    [SerializeField] private AudioClip beesEnterHive;
    [SerializeField] private AudioClip beesExitHive;
    private AudioSource audioSource;

    private Vector3 beehivePos;
    private Vector3 mostRecentPlayerPos;

    private bool isChasing = false;

    private Coroutine chaseCoroutine;
    private Coroutine goBackToBeehiveCoroutine;

    private void Awake()
    {
        beehivePos = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = true;

            if (goBackToBeehiveCoroutine != null)
            {
                StopCoroutine(goBackToBeehiveCoroutine);
                goBackToBeehiveCoroutine = null;
            }

            if (chaseCoroutine == null)
            {
                if (!bees.activeSelf)
                {
                    audioSource.PlayOneShot(beesExitHive);
                }
                bees.SetActive(true);
                chaseCoroutine = StartCoroutine(Chase());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = false;

            if (chaseCoroutine != null)
            {
                StopCoroutine(chaseCoroutine);
                chaseCoroutine = null;
            }

            if (goBackToBeehiveCoroutine == null)
            {
                goBackToBeehiveCoroutine = StartCoroutine(GoBackToBeehive());
            }
        }
    }

    private IEnumerator Chase()
    {
        Vector3 targetPosition = mostRecentPlayerPos;
        while (isChasing)
        {
            targetPosition = mostRecentPlayerPos;
            beesTransform.position = Vector3.MoveTowards(beesTransform.position, targetPosition, beesMovementSpeed * Time.deltaTime);
            yield return null;
        }
        chaseCoroutine = null;
    }

    private IEnumerator GoBackToBeehive()
    {
        while (Vector3.Distance(beesTransform.position, beehivePos) > 0.1f)
        {
            if (isChasing)
            {
                goBackToBeehiveCoroutine = null;
                yield break;
            }
            beesTransform.position = Vector3.Lerp(beesTransform.position, beehivePos, beesMovementSpeed * Time.deltaTime);
            yield return null;
        }
        
        beesTransform.position = beehivePos;
        yield return new WaitForSeconds(0.2f);
        bees.SetActive(false);
        audioSource.PlayOneShot(beesEnterHive);
        goBackToBeehiveCoroutine = null;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            mostRecentPlayerPos = playerTransform.position;
        }
    }
}
