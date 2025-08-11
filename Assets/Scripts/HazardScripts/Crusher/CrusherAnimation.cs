using UnityEngine;
using System.Collections;

public class CrusherAnimation : MonoBehaviour
{
    [SerializeField] private Animator leftCrusherAnimator;
    [SerializeField] private Animator rightCrusherAnimator;
    [SerializeField] private float minSmashDelay;
    [SerializeField] private float maxSmashDelay;

    private Coroutine crusherCoroutine;

    private void Awake()
    {
        crusherCoroutine = StartCoroutine(CrushingLoop());
    }

    private void OnDestroy()
    {
        StopCoroutine(crusherCoroutine);
    }

    private IEnumerator CrushingLoop()
    {
        while (true)
        {
            float finalSmashDelay = Random.Range(minSmashDelay, maxSmashDelay);
            yield return new WaitForSeconds(finalSmashDelay);
            leftCrusherAnimator.SetTrigger("Smash");
            rightCrusherAnimator.SetTrigger("Smash");
        }
    }
}
