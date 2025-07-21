using UnityEngine;

public class MiddleFinger : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 2f;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y + 1.4f, transform.position.z);
            isMoving = true;
            circleCollider.enabled = false;
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
