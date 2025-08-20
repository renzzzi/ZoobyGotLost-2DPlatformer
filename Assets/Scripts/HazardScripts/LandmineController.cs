using UnityEngine;
using System.Collections;

public class LandmineController : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private float explosionDelay;

    [Header("Landmine Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip landmineBeep;
    [SerializeField] private AudioClip landmineExplosion;

    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private Animator animator;
    private TickDamage tickDamageScript;

    private Coroutine explodeCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        tickDamageScript = GetComponent<TickDamage>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boxCollider.enabled = false;
            if (explodeCoroutine == null)
            {
                explodeCoroutine = StartCoroutine(Explosion());
            }
        }
    }

    private IEnumerator Explosion()
    {
        if (explodeCoroutine != null) yield break;

        animator.SetTrigger("Stepped");
        audioSource.PlayOneShot(landmineBeep);

        yield return new WaitForSeconds(explosionDelay);

        audioSource.PlayOneShot(landmineExplosion);
        tickDamageScript.ToggleDealDamageInstantly();
        tickDamageScript.enabled = true;
        circleCollider.enabled = true;
        Instantiate(explosionParticle, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.1f);
        circleCollider.enabled = false;

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        explodeCoroutine = null;
    }
}
