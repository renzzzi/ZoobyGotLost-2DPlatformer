using UnityEngine;

public class VentWindMovement : MonoBehaviour
{
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private float minMovementSpeed;
    [SerializeField] private float maxMovementSpeed;
    private float finalMovementSpeed;


    private void Awake()
    {
        finalMovementSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);
    }

    private void Update()
    {
        transform.Translate(moveDirection.normalized * finalMovementSpeed * Time.deltaTime);
    }
}
