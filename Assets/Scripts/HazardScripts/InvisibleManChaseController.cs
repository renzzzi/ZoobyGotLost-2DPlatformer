using UnityEngine;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;


public class InvisibleManChaseController : MonoBehaviour
{
    private enum State { Patrol, Chase }
    private State currentState;

    [Header("Player Detection Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector2 playerBoxDetectionSize;
    [SerializeField] private LayerMask playerLayer;

    [Header("Entity Collision Settings")]
    [SerializeField] private CapsuleCollider2D solidCollider;
    [SerializeField] private CapsuleCollider2D hitboxCollider;
    private Rigidbody2D rigidBody;

    [Header("Movement Settings")]
    [SerializeField] private float patrolMoveSpeed;
    [SerializeField] private float chaseMoveSpeed;

    [Header("Ledge Check Settings")]
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] private Vector2 ledgeCheckSize;

    [Header("Wall Check Settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    [Header("Ground Layer Mask")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Audio Step Settings")]
    [SerializeField] private float patrolStepRate;
    [SerializeField] private float chaseStepRate;
    [SerializeField] private float stepRateMultiplier;
    [SerializeField] private AudioClip walkGravelAudio;
    private float lastStepTime;
    private AudioSource audioSource;

    // Internal Variables
    private Coroutine patrolCoroutine;
    private int movingDirection = 1;
    private bool isNearLedge = false;
    private bool hasWallInFront = false;

    private Vector2 TransformCenter
    {
        get
        {
            return (Vector2)transform.position + new Vector2(0f, 0.5f);
        }
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.freezeRotation = true;
        audioSource = GetComponent<AudioSource>();
        currentState = State.Patrol;
        patrolCoroutine = StartCoroutine(EnemyPatrol());
    }

    private void Update()
    {
        bool inRange = Physics2D.OverlapBox(TransformCenter, playerBoxDetectionSize, 0f, playerLayer);
        bool checkForLedge = !Physics2D.OverlapBox(ledgeCheck.position, ledgeCheckSize, 0f, groundLayer);
        bool checkForWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, groundLayer);
  

        isNearLedge = checkForLedge;
        hasWallInFront = checkForWall;

        if (inRange)
        {
            ChangeState(State.Chase);
        }
        else
        {
            ChangeState(State.Patrol);
        }

        // Step audio
        float currentMoveSpeed = currentState == State.Patrol ? patrolMoveSpeed : chaseMoveSpeed;

        if (currentMoveSpeed > 0)
        {
            float finalStepInterval = stepRateMultiplier / currentMoveSpeed;

            if (Time.time - lastStepTime > finalStepInterval)
            {
                audioSource.PlayOneShot(walkGravelAudio);
                lastStepTime = Time.time;
            }
        }
    }

    private void FixedUpdate()
    {
        float finalMoveSpeed = (currentState == State.Patrol) ? patrolMoveSpeed : chaseMoveSpeed;
        rigidBody.linearVelocity = new Vector2(finalMoveSpeed * movingDirection, rigidBody.linearVelocity.y);
    }

    private void OnDestroy()
    {
        StopCoroutine(patrolCoroutine);
    }

    private void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;
    }

    private IEnumerator EnemyPatrol()
    {
        Debug.Log("eyyyy");
        while (true)
        {
            Debug.Log("breehhh");
            if (isNearLedge || hasWallInFront)
            {
                movingDirection *= -1;
                Flip();
                Debug.Log("lmao");
            }
            yield return null;
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void OnDrawGizmos()
    {
        if (ledgeCheck == null || wallCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(ledgeCheck.position, ledgeCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(TransformCenter, playerBoxDetectionSize);

    }
}