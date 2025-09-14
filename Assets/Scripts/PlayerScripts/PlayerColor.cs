using UnityEngine;
using System.Collections;

public class PlayerColor : MonoBehaviour
{

    private Coroutine hurtColorCoroutine;
    private Coroutine healColorCoroutine;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerStats.Instance.OnDamageInflicted += TriggerPlayerHurtColor;
        PlayerStats.Instance.OnPlayerHealed += TriggerPlayerHealColor;
    }

    private void TriggerPlayerHurtColor(int _)
    {
        if (hurtColorCoroutine == null)
        {
            hurtColorCoroutine = StartCoroutine(HurtColor());
        }
    }

    private void TriggerPlayerHealColor(int _)
    {
        if (healColorCoroutine == null)
        {
            healColorCoroutine = StartCoroutine(HealColor());
        }
    }

    private IEnumerator HealColor()
    {
        spriteRenderer.color = Color.green;
        yield return new WaitForSecondsRealtime(0.3f);
        spriteRenderer.color = Color.white;
        healColorCoroutine = null;
    }

    private IEnumerator HurtColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(0.2f);
        spriteRenderer.color = Color.white;
        hurtColorCoroutine = null;
    }
}
