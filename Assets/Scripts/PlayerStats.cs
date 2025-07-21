using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private GameUIController gameUIController;
    [SerializeField] private static int keyAmount = 0;
    [SerializeField] private static float health = 100.0f;
    private bool isDead = false;
    private int activeHazardTriggers = 0;
    private Coroutine damageCoroutine;
    private Coroutine playerHurtColorCoroutine;

    public static int GetKeyAmount()
    {
        return keyAmount;
    }

    public static float GetHealth()
    {
        return health;
    }

    public static event Action<int> OnKeyCollect;

    public void AddKey()
    {
        ++keyAmount;
        OnKeyCollect?.Invoke(keyAmount);
    }


    public static event Action<float> OnDamageInflicted;
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
        StopCoroutine(playerHurtColorCoroutine);
    }

    public static event Action OnPlayerDeath;
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
