using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class CausticFumesFade : MonoBehaviour
{
    [SerializeField] private float fadeInDuration;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float invisibilityDuration;
    [SerializeField] private float maxAlphaValue;
    private Tilemap tilemap;
    private Coroutine fadeLoopCoroutine;


    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        fadeLoopCoroutine = StartCoroutine(FadeLoop());
    }

    private void OnDestroy()
    {
        StopCoroutine(fadeLoopCoroutine);
    }

    private IEnumerator FadeLoop()
    {
        Coroutine fadeInCoroutine;
        Coroutine fadeOutCoroutine;

        while (gameObject.activeSelf)
        {
            fadeInCoroutine = StartCoroutine(FadeIn());
            yield return new WaitForSeconds(fadeInDuration);
            StopCoroutine(fadeInCoroutine);

            yield return null;

            fadeOutCoroutine = StartCoroutine(FadeOut());
            yield return new WaitForSeconds(fadeOutDuration);
            StopCoroutine(fadeOutCoroutine);

            yield return new WaitForSeconds(invisibilityDuration);
        }
    }

    private IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeOutDuration)
        {
            float newAlpha = Mathf.Lerp(maxAlphaValue, 0f, timeElapsed / fadeOutDuration);
            Color newColor = new(1f, 1f, 1f, newAlpha);
            tilemap.color = newColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        tilemap.color = new(1f, 1f, 1f, 0f);
    }

    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeInDuration)
        {
            float newAlpha = Mathf.Lerp(0f, maxAlphaValue, timeElapsed / fadeInDuration);
            Color newColor = new(1f, 1f, 1f, newAlpha);
            tilemap.color = newColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        tilemap.color = new(1f, 1f, 1f, maxAlphaValue);
    }
}
