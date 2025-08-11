using UnityEngine;

public class EntityFlip : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private void Update()
    {
        float directionToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
        if (directionToPlayer <= 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(+transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
