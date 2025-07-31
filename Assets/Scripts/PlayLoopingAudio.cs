using UnityEngine;

public class PlayLoopingAudio : MonoBehaviour
{
    [Tooltip("Drag an audio source here that loops.")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
