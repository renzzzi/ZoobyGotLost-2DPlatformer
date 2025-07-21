using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float parallaxEffect;

    private Vector3 startPosition;
    private float length;

    public void Awake()
    {
        startPosition = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void LateUpdate()
    {
        float distanceToMove = cameraTransform.position.x * parallaxEffect;

        Vector3 newPosition = new Vector3(startPosition.x + distanceToMove, transform.position.y, transform.position.z);
        transform.position = newPosition;

        float temp = cameraTransform.position.x * (1 - parallaxEffect);

        if (temp > startPosition.x + length)
        {
            startPosition.x += length;
        }
        else if (temp < startPosition.x - length)
        {
            startPosition.x -= length;
        }
    }
}