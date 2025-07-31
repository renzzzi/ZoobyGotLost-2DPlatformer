using UnityEngine;
using System.Collections;

public class PlayerHurtColor : MonoBehaviour
{

    private Coroutine hurtColorCoroutine;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerStats.Instance.OnDamageInflicted += TriggerPlayerHurtColor;
    }

    private void TriggerPlayerHurtColor(float _)
    {
        if (hurtColorCoroutine == null)
        {
            StartCoroutine(HurtColor());
        }
    }

    private IEnumerator HurtColor()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(0.2f);
        spriteRenderer.color = Color.white;
        hurtColorCoroutine = null;
    }
}
