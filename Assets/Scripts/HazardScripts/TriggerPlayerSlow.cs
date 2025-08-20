using UnityEngine;

public class TriggerPlayerSlow : MonoBehaviour
{
    [Header("Slowdown Settings")]
    [Tooltip("How long the slowdown effect should last in seconds.")]
    [SerializeField] private float slowDuration;

    [Tooltip("The speed multiplier to apply. 0.5 means 50% speed. 0.2 means 20% speed.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float slowMagnitude;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.TriggerSlowPlayer(slowDuration, slowMagnitude);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.StopSlowPlayer();
        }
    }
}