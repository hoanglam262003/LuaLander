using UnityEngine;

public class BulletCannon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 20f;

    private Rigidbody2D rb;
    private Collider2D bulletCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<FuelPickUp>() != null || other.GetComponent<Coins>() != null || other.GetComponent<TowerDetection>() != null || other.GetComponent<BulletCannon>() != null)
        {
            Physics2D.IgnoreCollision(bulletCollider, other);
            return;
        }
        Destroy(gameObject);
    }
}