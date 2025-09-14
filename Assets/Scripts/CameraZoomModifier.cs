using UnityEngine;
using System.Collections;

public class CameraZoomModifier : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraZoom;
    [SerializeField] private float cameraZoomSpeed;
    private Coroutine zoomCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && 
            mainCamera.orthographicSize != cameraZoom)
        {
            zoomCoroutine = StartCoroutine(Zoom());
        }
    }

    private IEnumerator Zoom()
    {
        float timeElapsed = 0f;
        while (timeElapsed < cameraZoomSpeed)
        {
            timeElapsed += Time.deltaTime;

            float progress = timeElapsed / cameraZoomSpeed;
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraZoom, progress);

            yield return null;
        }
        mainCamera.orthographicSize = cameraZoom;
        zoomCoroutine = null;
    }
}
