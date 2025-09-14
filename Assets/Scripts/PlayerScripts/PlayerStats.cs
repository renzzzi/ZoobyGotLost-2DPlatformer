using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private Animator playerAnimator;
    private bool isDead = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentHealth = maxHealth;
        playerAnimator = GetComponent<Animator>();
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void SetHealth(int newHealth)
    {
        this.currentHealth = newHealth;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public event Action<int> OnDamageInflicted;
    public void InflictDamage(int damageAmount)
    {

        currentHealth -= damageAmount;
        OnDamageInflicted?.Invoke(currentHealth);

        AudioManager.Instance.PlaySFX(SoundType.Hurt);
        CameraShake.Instance.TriggerShake();

        if (currentHealth <= 0)
        {
            InitiateDeath();
        }
    }

    public event Action<int> OnPlayerHealed;
    public void AddHealth(int newHealth)
    {
        AudioManager.Instance.PlaySFX(SoundType.Heal);
        currentHealth += newHealth;
        OnPlayerHealed?.Invoke(currentHealth);
    }

    public event Action OnPlayerDeath;
    public event Action AfterPlayerDeathAnim;

    private void InitiateDeath()
    {
        // Set Player State
        isDead = true;

        // Invoke Player Death Immediately
        OnPlayerDeath?.Invoke();

        // Disable Player Input
        GetComponent<PlayerController>().enabled = false;
        // Resets the linear velocity both x and y of the rigid body to zero
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        // Set body type to kinematic so the body will stay put even in mid-air
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
  
        // Hide HUD
        gameUIController.HideHUD();
        // Stop BG Music and Plays GameOver Music
        AudioManager.Instance.StopMusic();

        // To nullify any animator speed manipulation from other scripts
        playerAnimator.speed = 1f;
        // Resets the YVelocity so that death animation will play even in mid-air
        GetComponent<Animator>().SetFloat("YVelocity", 0);
        // Trigger Death Animation
        GetComponent<Animator>().SetTrigger("Death");

    }

    public void HidePlayerBody()
    {
        // After death animation ends, stop showing the player
        GetComponent<SpriteRenderer>().enabled = false;
        /* Invoke this event for anything that will happen after the death animation like
         * the game over menu.
         */
        AfterPlayerDeathAnim?.Invoke();
    }
}
