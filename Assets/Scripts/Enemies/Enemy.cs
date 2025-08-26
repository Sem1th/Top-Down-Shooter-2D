using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int health = 4;
    [SerializeField] private int touchDamage = 1;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private Transform spriteTransform;

    private Rigidbody2D _rb;
    private Transform _player;
    private GameManager _gm;

    public void Init(Transform player, GameManager gm)
    {
        _player = player;
        _gm = gm;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
    }

    private void Update()
    {
        if (_player == null) return;

        Vector2 dir = ((Vector2)_player.position - (Vector2)transform.position).normalized;
        _rb.velocity = dir * speed;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            _gm?.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _gm?.DamagePlayer(touchDamage);
            Destroy(gameObject);
        }
    }
}
