using UnityEngine;

public class PlayAudioDuringAnim : MonoBehaviour
{
    private AudioSource audioSource;
    [Tooltip("Plays an audio at anytime in the animation by an animation event.")]
    [SerializeField] AudioClip playAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    public void PlaySound()
    {
        if (playAudioClip != null)
        {
            audioSource.PlayOneShot(playAudioClip);
        }
        else
        {
            Debug.Log("Audio File Missing!");
        }
    }
}
