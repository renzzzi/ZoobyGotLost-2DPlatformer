using UnityEngine;

public class RedRubberDuck : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 2f;
    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z);
            isMoving = true;
            capsuleCollider.enabled = false;
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }
}
