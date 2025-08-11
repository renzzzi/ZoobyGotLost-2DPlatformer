using Unity.VisualScripting;
using UnityEngine;

public class VentWindDeletion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StrongVentWind"))
        {
            Destroy(collision.gameObject);
        }
    }
}
