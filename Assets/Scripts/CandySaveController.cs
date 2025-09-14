using UnityEngine;
using UnityEngine.InputSystem;

public class CandySaveController : MonoBehaviour
{
    [SerializeField] private Vector2 interactBoxSize;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float finalYCenterPosition;
    
    private Vector2 finalCenterPosition;
    private InputAction interactButton;
    
    private PlayerInput playerInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private bool savedAlready = false;

    private void Awake()
    {
        
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        interactButton = playerInput.actions.FindAction("Interact");
        finalCenterPosition = new(transform.position.x + 0.125f, transform.position.y + finalYCenterPosition);
    }

    private void Update()
    {
        bool isPlayerInRange = Physics2D.OverlapBox(finalCenterPosition, interactBoxSize, 0f, playerLayer);

        if (isPlayerInRange && interactButton.WasPressedThisFrame() && !savedAlready)
        {
            animator.SetTrigger("Save");
            AudioManager.Instance.PlaySFX(SoundType.Save);
            spriteRenderer.color = Color.greenYellow;
            SaveManager.Instance.SaveGame(transform.position);
            savedAlready = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gold;
        Gizmos.DrawWireCube(finalCenterPosition, interactBoxSize);
    }
}
