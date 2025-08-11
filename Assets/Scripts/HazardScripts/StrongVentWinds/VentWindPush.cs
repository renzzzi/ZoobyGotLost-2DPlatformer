using UnityEngine;

public class VentWindPush : MonoBehaviour
{
    [Tooltip("(-1, 0) = Left, (1, 0) = Right")]
    [SerializeField] private Vector2 pushDirection;
    [SerializeField] private float pushMagnitude;

    private bool isInPushArea = false;
    private Rigidbody2D playerRigidBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerRigidBody = collision.GetComponent<Rigidbody2D>();
        isInPushArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInPushArea = false;
        playerRigidBody = null;
    }

    private void FixedUpdate()
    {
        if (isInPushArea && playerRigidBody != null)
        {
            playerRigidBody.AddForce(pushDirection * pushMagnitude);
        }
    }
}
