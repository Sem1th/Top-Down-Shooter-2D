using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
[Header("Bullet Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    public int Damage { get; set; } 
    private Rigidbody2D _rb;
    private float _lifeTimer;
    private Vector2 _direction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _lifeTimer = lifetime;
    }

    public void Fire(Vector2 direction)
    {
        _direction = direction.normalized;
        if (_rb != null)
            _rb.velocity = _direction * speed;
    }

    private void Update()
    {
        _lifeTimer -= Time.deltaTime;
        if (_lifeTimer <= 0)
            Despawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(1);

            Despawn();
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        _rb.velocity = Vector2.zero;
        Destroy(gameObject); 
    }
}
