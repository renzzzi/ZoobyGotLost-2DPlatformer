using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Tooltip("The effect strength. 0 = no parallax (background is stationary). 1 = full parallax (background moves with the player).")]
    [Range(0f, 1f)]
    [SerializeField] private float parallaxEffect;

    private Transform playerTransform;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        playerTransform = transform.parent;

        lastPlayerPosition = playerTransform.position;
    }

    void Update()
    {
        Vector3 backgroundMovement = playerTransform.position - lastPlayerPosition;

        transform.position -= backgroundMovement * (1 - parallaxEffect);

        lastPlayerPosition = playerTransform.position;
    }
}