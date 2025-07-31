using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class QuicksandDeath : MonoBehaviour
{
    [Header("Sinking Animation")]
    [Tooltip("How far down the player should sink.")]
    [SerializeField] private float sinkDistance;
    [Tooltip("How long the sinking animation should take in seconds.")]
    [SerializeField] private float sinkDuration;

    [Header("Quicksand Audio")]
    [SerializeField] private AudioClip quicksandHit;
    [SerializeField] private AudioClip quicksandSinking;
    private AudioSource audioSource;

    private Coroutine sinkingCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (sinkingCoroutine != null)
            {
                StopCoroutine(sinkingCoroutine);
            }
            sinkingCoroutine = StartCoroutine(SinkingAnimation(collision.GetComponent<Rigidbody2D>()));
        }
    }

    private IEnumerator SinkingAnimation(Rigidbody2D playerRigidBody)
    {
        audioSource.PlayOneShot(quicksandHit);
        audioSource.PlayOneShot(quicksandSinking);

        playerRigidBody.bodyType = RigidbodyType2D.Kinematic;
        playerRigidBody.linearVelocity = Vector2.zero;

        Vector3 startPosition = playerRigidBody.transform.position;
        Vector3 endPosition = startPosition - new Vector3(0, sinkDistance, 0);

        float timeElapsed = 0f;

        while (timeElapsed < sinkDuration)
        {
            float temp = timeElapsed / sinkDuration;

            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, temp);

            playerRigidBody.MovePosition(newPosition);

            timeElapsed += Time.deltaTime;
            yield return null;
        }


        playerRigidBody.MovePosition(endPosition);


        PlayerStats.Instance.InflictDamage(100);

        sinkingCoroutine = null;
    }
}