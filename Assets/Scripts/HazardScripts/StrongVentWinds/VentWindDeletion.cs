using UnityEngine;

public class VentWindDeletion : MonoBehaviour
{
    private enum StrongVentWindDirection
    { 
        StrongVentWindLeft, StrongVentWindRight
    }

    [SerializeField] private StrongVentWindDirection windDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string finalDirection = windDirection.ToString();
        if (collision.gameObject.CompareTag(finalDirection))
        {
            Destroy(collision.gameObject);
        }
    }
}
