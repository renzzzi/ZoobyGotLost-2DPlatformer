using UnityEngine;

public class TriggerSlowPlayer : MonoBehaviour
{
    [Header("Slowdown Settings")]
    [Tooltip("How long the slowdown effect should last in seconds.")]
    [SerializeField] private float slowDuration;

    [Tooltip("The speed multiplier to apply. 0.5 means 50% speed. 0.2 means 20% speed.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float slowMagnitude;

    [Header("Trigger Behavior")]
    [Tooltip("If checked, this trigger will only activate once.")]
    [SerializeField] private bool triggerOnce = false;

    private bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerOnce && hasBeenTriggered)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.TriggerSlowPlayer(slowDuration, slowMagnitude);

            if (triggerOnce)
            {
                hasBeenTriggered = true;
            }
        }
    }
}