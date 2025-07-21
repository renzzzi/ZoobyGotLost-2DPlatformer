using UnityEngine;
using System;

public class DoorPortal : MonoBehaviour
{
    private CapsuleCollider2D portalCollider;
    private Animator animator;
    [SerializeField] private int keyRequired = 5;
    private bool isDoorOpened = false;

    public void Awake()
    {
        portalCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        portalCollider.enabled = false;
    }

    private void Update()
    {
        if (isDoorOpened)
        {
            return;
        }

        int currentKeyAmount = PlayerStats.GetKeyAmount();

        if (currentKeyAmount >= keyRequired)
        {
            isDoorOpened = true;
            animator.SetBool("isOpened", isDoorOpened);
            portalCollider.enabled = true;
            AudioManager.Instance.PlaySFX(SoundType.PortalOpen);
        }
    }

    public static event Action OnPlayerWin;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerWin?.Invoke();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(SoundType.Win);
            this.enabled = false;
        }
    }
}
