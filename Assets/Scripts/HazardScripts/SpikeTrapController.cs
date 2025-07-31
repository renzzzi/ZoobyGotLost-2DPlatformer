using UnityEngine;
using System.Collections;

public class SpikeTrapController : MonoBehaviour
{
    private Animator animator;
    private Coroutine spikeTrapLoopCoroutine;
    private CapsuleCollider2D capsuleCollider;

    [Header("The duration of each spike trap animation")]
    [SerializeField] private float warningDuration;
    [SerializeField] private float upDuration;
    [SerializeField] private float downDuration;

    private AudioSource audioSource;
    [SerializeField] private AudioClip spikeTrapUp;
    [SerializeField] private AudioClip spikeTrapDown;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        if (spikeTrapLoopCoroutine == null)
        {
            spikeTrapLoopCoroutine = StartCoroutine(SpikeTrapLoop());
        }
    }

    private void OnDestroy()
    {
        if (spikeTrapLoopCoroutine != null)
        {
            StopCoroutine(spikeTrapLoopCoroutine);
        }
    }

    private IEnumerator SpikeTrapLoop()
    {
        while (gameObject.activeSelf)
        {
            // Randomizes the length of each interval
            float randomNumber = Random.Range(0.2f, 0.8f);
            yield return new WaitForSeconds(randomNumber);

            animator.SetTrigger("Warning");
            yield return new WaitForSeconds(warningDuration);

            animator.SetTrigger("Up");
            capsuleCollider.enabled = true;
            audioSource.PlayOneShot(spikeTrapUp);
            yield return new WaitForSeconds(upDuration);


            animator.SetTrigger("Down");
            capsuleCollider.enabled = false;
            audioSource.PlayOneShot(spikeTrapDown);
            yield return new WaitForSeconds(downDuration);

            animator.SetTrigger("Default");
        }
    }
}
