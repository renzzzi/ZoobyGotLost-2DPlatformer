using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float magnitude = 0.1f;

    private Vector3 baseLocalPosition;

    private Vector3 shakeOffset = Vector3.zero;

    private Coroutine shakeCoroutine;

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

    private void Start()
    {
        baseLocalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        transform.localPosition = baseLocalPosition + shakeOffset;
    }

    public void TriggerShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }
        shakeCoroutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0f);

            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
        shakeCoroutine = null;
    }
}