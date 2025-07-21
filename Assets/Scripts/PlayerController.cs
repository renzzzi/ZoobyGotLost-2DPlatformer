using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;

    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);

    [Header("Jump Tuning")]
    [Tooltip("Allows jumping for a short time after leaving a ledge.")]
    [SerializeField] private float coyoteTime;
    [Tooltip("Remembers a jump input for a short time before hitting the ground.")]
    [SerializeField] private float jumpBufferTime;

    [Header("Walking SFX Settings")]
    [SerializeField] private float walkCooldown;
    private float walkTimer;
    private bool onGroundHitFlag = false;

    private float moveInput;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isJumping = false;

    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.freezeRotation = true;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>().x;
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            isJumping = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpAction.WasPressedThisFrame())
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        HandleSpriteFlip();
        HandleAnimations();
        HandleWalkingSound();
        HandleGroundHitSound();
    }

    private void FixedUpdate()
    {
        rigidBody.linearVelocity = new Vector2(moveInput * speed, rigidBody.linearVelocity.y);

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, jumpForce);
            isJumping = true;
            AudioManager.Instance.PlaySFX(SoundType.Jump);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // If the player releases the jump button early (or never held it), cut the jump short.
        if (isJumping && !jumpAction.IsPressed() && rigidBody.linearVelocity.y > 0f)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, rigidBody.linearVelocity.y * 0.4f);
            // By setting isJumping to false, we prevent this from running again on the same jump
            isJumping = false;
        }
    }

    private void HandleSpriteFlip()
    {
        if (Time.timeScale >= 0.1f)
        {
            if (moveInput > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void HandleAnimations()
    {
        if (Mathf.Abs(rigidBody.linearVelocity.x) < 0.1f)
        {
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }

        animator.SetFloat("YVelocity", rigidBody.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void HandleWalkingSound()
    {
        walkTimer += Time.deltaTime;

        if (isGrounded && Mathf.Abs(rigidBody.linearVelocity.x) > 0.05f)
        {
            if (walkTimer > walkCooldown)
            {
                AudioManager.Instance.PlaySFX(SoundType.WalkGrass);
                walkTimer = 0f;
            }
        }
    }

    private void HandleGroundHitSound()
    {
        if (onGroundHitFlag && isGrounded)
        {
            AudioManager.Instance.PlaySFX(SoundType.GroundHitGrass);
            onGroundHitFlag = false;
        }
        else
        {
            if (rigidBody.linearVelocity.y < -0.05f && !isGrounded)
            {
                onGroundHitFlag = true;
            }
        }
    }

    // Draws a wireframe box in the editor to visualize the ground check area
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}