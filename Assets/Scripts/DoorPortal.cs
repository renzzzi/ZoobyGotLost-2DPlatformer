using UnityEngine;
using System;

public class DoorPortal : MonoBehaviour
{
    public static event Action OnPlayerWin;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip portalWhooshing;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerWin?.Invoke();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(SoundType.Win);
            AudioManager.Instance.musicSource.volume = 0.2f;
            this.enabled = false;
        }
    }

    public void PlayWhooshingSound()
    {
        audioSource.PlayOneShot(portalWhooshing);
    }
}
