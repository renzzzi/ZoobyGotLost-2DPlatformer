using System;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private float health;
    private int keyAmount;
    private bool isDead = false;
    private int activeHazardTriggers = 0;
    private Coroutine damageCoroutine;
    private Coroutine playerHurtColorCoroutine;

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
    }

    public int GetKeyAmount()
    {
        return keyAmount;
    }

    public float GetHealth()
    {
        return health;
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
        playerHurtColorCoroutine = StartCoroutine(PlayerHurtColor());
    }

    public void EnterHazard(int minDamage, int maxDamage, float damageInterval)
    {
        activeHazardTriggers++;

        if (activeHazardTriggers == 1)
        {
            damageCoroutine = StartCoroutine(DamageOverTime(minDamage, maxDamage, damageInterval));
        }
    }

    public void ExitHazard()
    {
        activeHazardTriggers--;

        if (activeHazardTriggers <= 0 && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
    }

    private IEnumerator DamageOverTime(int minDamage, int maxDamage, float damageInterval)
    {
        while (!isDead)
        {
            int randomDamage = UnityEngine.Random.Range(minDamage, maxDamage);

            InflictDamage(randomDamage);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private IEnumerator PlayerHurtColor()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSecondsRealtime(0.2f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public event Action OnPlayerDeath;
    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            // Initiate Death Animation
            // Disable Player Input
            GetComponent<PlayerController>().enabled = false;
            // Disable Rigid Body
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<Animator>().SetFloat("YVelocity", 0);
            GetComponent<Animator>().SetTrigger("Death");
            // Hide HUD
            gameUIController.HideHUD();
            // Stop BG Music and Plays GameOver Music
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX(SoundType.GameOver);
        }
    }

    public void InitiateDeath()
    {
        OnPlayerDeath?.Invoke();
        GetComponent<SpriteRenderer>().enabled = false;    
    }
}
