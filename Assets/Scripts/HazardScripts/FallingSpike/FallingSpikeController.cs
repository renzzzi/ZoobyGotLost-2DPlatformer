using UnityEngine;
using System.Collections;

public class FallingSpikeController : MonoBehaviour
{
    [Tooltip("How long the spike will gradually fade out")]
    [SerializeField] private float fadeOutDuration;

    [SerializeField] private LayerMask groundLayer;
    private Coroutine spikeFadeOutCoroutine;

    [SerializeField] private AudioClip audioClipOnHit;
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << 3) & groundLayer) != 0)
        {
            audioSource.PlayOneShot(audioClipOnHit);
            spikeFadeOutCoroutine = StartCoroutine(SpikeFadeOut());
        }
    }

    private IEnumerator SpikeFadeOut()
    {
        yield return new WaitForSeconds(1f);

        float timeElapsed = 0f;
        Color initialColor = spriteRenderer.color;
        float initialAlpha = initialColor.a;
        float targetAlpha = 0f;

        while (timeElapsed < fadeOutDuration)
        {
            timeElapsed += Time.deltaTime;

            float progress = timeElapsed / fadeOutDuration;
            float newAlpha = Mathf.Lerp(initialAlpha, targetAlpha, progress);

            spriteRenderer.color = new Color(initialColor.r, initialColor.g,
                                             initialColor.b, newAlpha);
            yield return null;
        }

        spriteRenderer.color = new Color(initialColor.r, initialColor.g,
                                             initialColor.b, targetAlpha);
        Destroy(gameObject);
        spikeFadeOutCoroutine = null;
    }
}
