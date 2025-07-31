using UnityEngine;

public class PlayAudioOnHit : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerStats.Instance.OnDamageInflicted += PlayAudio;
    }

    private void OnDisable()
    {
        PlayerStats.Instance.OnDamageInflicted -= PlayAudio;
    }

    private void PlayAudio(float _)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
