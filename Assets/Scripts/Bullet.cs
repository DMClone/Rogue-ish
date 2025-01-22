using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isPlayerOwned;
    public Vector2 direction;
    public float speed;
    private Rigidbody2D _rigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        transform.up = direction;
        _rigidbody.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.GetComponent<Health>() != null) && other.GetComponent<Health>().isPlayer != isPlayerOwned)
        {
            other.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
