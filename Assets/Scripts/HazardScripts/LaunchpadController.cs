using UnityEngine;
using System;

public class LaunchpadController : MonoBehaviour
{
    public static event Action<float> OnLaunch;

    [SerializeField] private AudioClip launchAudio;
    [SerializeField] private float launchMagnitude;
    private AudioSource audioSource;
    private Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(launchAudio);
            animator.SetTrigger("Launch");
            OnLaunch?.Invoke(launchMagnitude);
        }
    }
}
