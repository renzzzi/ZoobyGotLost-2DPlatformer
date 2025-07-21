using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    [SerializeField] private Animator keyAnimator;
    private bool isCollected = false;
    private BoxCollider2D collider2d;

    public void Awake()
    {
        collider2d = GetComponent<BoxCollider2D>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            isCollected = true;
            if (playerStats != null)
            {
                AudioManager.Instance.PlaySFX(SoundType.KeyCollect);
                collider2d.enabled = false;
                playerStats.AddKey();

                if (keyAnimator != null)
                {
                    keyAnimator.SetBool("isCollected", isCollected);
                }
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

