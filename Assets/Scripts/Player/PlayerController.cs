using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private int maxHealth = 3;

    [Header("Shooting")]
    [SerializeField] private PlayerShooting shootingSystem; 
    
    private int _health;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private InputActions _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = maxHealth;

        _input = new InputActions();
        _input.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Move.canceled += ctx => _moveInput = Vector2.zero;

        _input.Player.Fire.performed += ctx => shootingSystem?.OnFire(ctx);
        _input.Player.Fire.canceled += ctx => shootingSystem?.OnFire(ctx);

        _input.Player.SwitchWeapon.performed += shootingSystem.OnSwitchWeapon;

        _input.Enable();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _moveInput * moveSpeed;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        _health = Mathf.Max(_health, 0);

        ServiceLocator.Resolve<UIManager>()?.UpdateHUD(
            _health,
            ServiceLocator.Resolve<GameManager>()?.Score ?? 0,
            ServiceLocator.Resolve<GameManager>()?.SurvivalTime ?? 0f
        );

        if (_health <= 0)
        {
            ServiceLocator.Resolve<GameManager>()?.GameOver();
            gameObject.SetActive(false);
        }
    }
}
