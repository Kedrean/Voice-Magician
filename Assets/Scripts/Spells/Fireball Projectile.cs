
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireballProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 2f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            return;

        Destroy(gameObject);
    }
}
