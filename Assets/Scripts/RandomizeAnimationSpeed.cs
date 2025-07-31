using UnityEngine;

public class RandomizeAnimationSpeed : MonoBehaviour
{
    private Animator animator;
    [Header("Set minimum and max animation speed")]
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        animator.speed = randomSpeed;
    }
}
