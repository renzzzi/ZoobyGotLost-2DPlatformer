using UnityEngine;
using System.Collections;

public class BGMVolumeModifier : MonoBehaviour
{
    private AudioSource bgmAudioSource;
    [Range(0f, 1f)]
    [SerializeField] private float volumeAdjustment;
    // Volume will slide to the desired volume rather than abruptly changing
    [SerializeField] private float volumeSlideSpeed;

    private Coroutine volumeSlideCoroutine;

    private void Start()
    {
        bgmAudioSource = AudioManager.Instance.musicSource;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            volumeAdjustment != bgmAudioSource.volume &&
            bgmAudioSource != null)
        {
            volumeSlideCoroutine = StartCoroutine(VolumeSlide());
        }
    }

    private IEnumerator VolumeSlide()
    {
        float timeElapsed = 0f;

        while (timeElapsed < volumeSlideSpeed)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / volumeSlideSpeed;
            bgmAudioSource.volume = Mathf.Lerp(bgmAudioSource.volume, volumeAdjustment, progress);

            yield return null;
        }
        bgmAudioSource.volume = volumeAdjustment;
        volumeSlideCoroutine = null;
    }
}
