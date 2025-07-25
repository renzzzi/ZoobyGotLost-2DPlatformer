using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    // All the components
    [Header("Components")]
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerInput playerInput;

    // Basic movement settings
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    // Stores all the needed components for checking the ground
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);

    // Stores all the settings that involves making the jumps feel good and responsive
    [Header("Jump Tuning")]
    [Tooltip("Allows jumping for a short time after leaving a ledge.")]
    [SerializeField] private float coyoteTime;
    [Tooltip("Remembers a jump input for a short time before hitting the ground.")]
    [SerializeField] private float jumpBufferTime;

    // Stores settings for the walking sound effect
    [Header("Walking SFX Settings")]
    [SerializeField] private float stepRate;
    private float lastStepTime = 0f;
    private bool onGroundHitFlag = false;

    /* Internal variables that stores crucial information, in order:
     * 1. The direction in where the player will go based on the input of the user.
     * 2. A flag telling whether the player is on the ground or not
     * 3. Counts down from coyoteTime
     * 4. Counts down from jumpBufferTime
     * 5. A flag telling whether the player is jumping or not (The player might be falling
     *    but isGrounded does not tell the whole story)
     * 6. Stores the input action mapping for moving left and right (Which is a Vector2 
     *    and the moveInput variable only stores the x-axis)
     * 7. Stores the input action mapping for the jump button
     */
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

    // InputAction variables apparently needs to be enabled
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

    // Events for SFX
    public static event Action OnPlayerStep;
    public static event Action OnGroundHit;

    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>().x;
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);

        // Logic for SFX
        if (isGrounded && Mathf.Abs(rigidBody.linearVelocity.x) > 0.1f)
        {
            if (Time.time - lastStepTime > stepRate)
            {
                OnPlayerStep?.Invoke();
                lastStepTime = Time.time;
            }
        }

        if (onGroundHitFlag && isGrounded)
        {
            OnGroundHit?.Invoke();
            onGroundHitFlag = false;
        }
        else if (rigidBody.linearVelocity.y < -0.05f && !isGrounded)
        {
            onGroundHitFlag = true;
        }

        // Logic for Coyote Time and Jump Buffer
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

    // Draws a wireframe box in the scene to visualize the ground check area
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }
}