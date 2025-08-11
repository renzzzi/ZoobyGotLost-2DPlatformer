using System.Collections;
using UnityEngine;

public class FlyChaseController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform chaserTransform;
    [SerializeField] private GameObject chaserObject;
    [SerializeField] private SpriteRenderer chaserSpriteRenderer;
    [SerializeField] private float movementSpeed;

    [SerializeField] private AudioClip returnSound;
    [SerializeField] private AudioClip chaseSound;
    [SerializeField] private AudioSource audioSource;

    private Vector3 startPosition;
    private Vector3 mostRecentPlayerPos;

    private bool isChasing = false;

    private Coroutine chaseCoroutine;
    private Coroutine returnCoroutine;
    

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = true;

            if (returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine);
                returnCoroutine = null;
            }

            if (chaseCoroutine == null)
            {
                if (!chaserObject.activeSelf)
                {
                    audioSource.PlayOneShot(chaseSound);
                }
                chaserObject.SetActive(true);
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

            if (returnCoroutine == null)
            {
                returnCoroutine = StartCoroutine(ReturnToStart());
            }
        }
    }

    private IEnumerator Chase()
    {
        Vector3 targetPosition = mostRecentPlayerPos;
        while (isChasing)
        {
            targetPosition = mostRecentPlayerPos;
            chaserTransform.position = Vector3.MoveTowards(chaserTransform.position, targetPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }
        chaseCoroutine = null;
    }

    private IEnumerator ReturnToStart()
    {
        while (Vector3.Distance(chaserTransform.position, startPosition) > 0.1f)
        {
            if (isChasing)
            {
                returnCoroutine = null;
                yield break;
            }
            chaserTransform.position = Vector3.Lerp(chaserTransform.position, startPosition, movementSpeed * Time.deltaTime);
            yield return null;
        }

        chaserTransform.position = startPosition;
        yield return new WaitForSeconds(0.2f);
        chaserObject.SetActive(false);
        audioSource.PlayOneShot(returnSound);
        returnCoroutine = null;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            mostRecentPlayerPos = playerTransform.position;
        }

        float directionToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
        if (directionToPlayer <= 0)
        {
            chaserSpriteRenderer.flipX = true;
        }
        else
        {
            chaserSpriteRenderer.flipX = false;
        }
    }
}