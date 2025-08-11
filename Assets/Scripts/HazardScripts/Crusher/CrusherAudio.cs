using UnityEngine;

public class CrusherAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip crusherWarning;
    [SerializeField] private AudioClip crusherImpact;

    public void PlayWarning()
    {
        audioSource.PlayOneShot(crusherWarning);
    }

    public void PlayImpact()
    {
        audioSource.PlayOneShot(crusherImpact);
    }
}
