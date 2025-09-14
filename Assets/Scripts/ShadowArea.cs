using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class ShadowArea : MonoBehaviour
{
    [SerializeField] private Tilemap areaTilemap;
    private Coroutine activeFadeCoroutine;

    // A public method for the manager to call
    public void Fade(float targetAlpha, float duration)
    {
        // If we are already fading, stop the old fade and start a new one
        if (activeFadeCoroutine != null)
        {
            StopCoroutine(activeFadeCoroutine);
        }
        activeFadeCoroutine = StartCoroutine(FadeCoroutine(targetAlpha, duration));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When the player enters, this area tells the manager, "I'm the new active area!"
        if (collision.CompareTag("Player"))
        {
            ShadowManager.Instance.SetCurrentArea(this);
        }
    }

    private IEnumerator FadeCoroutine(float targetAlpha, float duration)
    {
        float timeElapsed = 0f;
        Color currentColor = areaTilemap.color;
        float initialAlpha = currentColor.a;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(timeElapsed / duration);

            currentColor.a = Mathf.Lerp(initialAlpha, targetAlpha, progress);
            areaTilemap.color = currentColor;
            yield return null;
        }

        // Finalize the color and mark the coroutine as done
        currentColor.a = targetAlpha;
        areaTilemap.color = currentColor;
        activeFadeCoroutine = null;
    }
}