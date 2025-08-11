using UnityEngine;

public class ProximityFade : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [Tooltip("Where the hazard reach maximum visibility")]
    [SerializeField] private float minVisibilityDistance;
    [Tooltip(" Where the hazard starts to become visible")]
    [SerializeField] private float maxVisibilityDistance;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float currentDistance = Vector3.Distance(transform.position, playerTransform.position);

        /* Calculates the percentage of completion from the starting point(FirstParam)
         * to the end point(SecondParam)
         */
        float progress = Mathf.InverseLerp(maxVisibilityDistance, minVisibilityDistance, currentDistance);

        // Simple copy the sprite renderer's current color
        Color currentColor = spriteRenderer.color;
        // Change the alpha value to the calculate percentage of completion
        currentColor.a = progress;
        // Apply the new alpha value to the actual sprite renderer
        spriteRenderer.color = currentColor;
    }
}
