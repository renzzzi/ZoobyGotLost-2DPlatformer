using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private float health;
    private Animator playerAnimator;
    private int keyAmount = 0;
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

        playerAnimator = GetComponent<Animator>();
    }

    public int GetKeyAmount()
    {
        return keyAmount;
    }

    public float GetHealth()
    {
        return health;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    public event Action<int> OnKeyCollect;

    public void AddKey()
    {
        ++keyAmount;
        OnKeyCollect?.Invoke(keyAmount);
    }


    public event Action<float> OnDamageInflicted;
    public void InflictDamage(int damageAmount)
    {

        health -= damageAmount;
        OnDamageInflicted?.Invoke(health);

        AudioManager.Instance.PlaySFX(SoundType.Hurt);
        CameraShake.Instance.TriggerShake();

        if (health <= 0)
        {
            InitiateDeath();
        }
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
