using UnityEngine;

public class VentWindMovement : MonoBehaviour
{
    [SerializeField] private Transform windDeletionTransform;
    [SerializeField] private float minMovementSpeed;
    [SerializeField] private float maxMovementSpeed;

    private void Update()
    {
        Vector2 targetPosition = new (windDeletionTransform.position.x, transform.position.y);
        float finalMovementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, finalMovementSpeed * Time.deltaTime);
    }
}
