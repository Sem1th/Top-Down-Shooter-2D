using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Sprite assaultRifleSprite;
    [SerializeField] private Sprite pistolSprite;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    private IWeapon _currentWeapon;
    private WeaponSystem _assaultRifle;
    private ShotgunWeapon _pistol;
    
    private bool _isFiring;
    private Vector2 _lastAimDirection;

    private void Awake()
    {
        // Инициализируем оружие
        _assaultRifle = new WeaponSystem(bulletPrefab, assaultRifleSprite);
        _pistol = new ShotgunWeapon(bulletPrefab, pistolSprite);
        
        // Передаем ссылку на систему стрельбы
        _assaultRifle.SetShootingSystem(this);
        
        _currentWeapon = _assaultRifle;
        UpdateWeaponSprite();
    }

    private void Update()
    {
        _currentWeapon.Tick(Time.deltaTime);

        if (_isFiring)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;

            _lastAimDirection = (worldPos - firePoint.position).normalized;
            
            _currentWeapon.TryFire(firePoint.position, _lastAimDirection);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && _currentWeapon != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0f;
            Vector2 aimDir = (worldPos - firePoint.position).normalized;

            _currentWeapon.TryFire(firePoint.position, aimDir);
        }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        // Переключаем оружие
        if (_currentWeapon == _assaultRifle)
        {
            _currentWeapon = _pistol;
        }
        else
        {
            _currentWeapon = _assaultRifle;
        }
        
        UpdateWeaponSprite();
        Debug.Log($"Switched to: {_currentWeapon.GetType().Name}");
    }
    
    private void UpdateWeaponSprite()
    {
        if (weaponSpriteRenderer != null && _currentWeapon.WeaponSprite != null)
        {
            weaponSpriteRenderer.sprite = _currentWeapon.WeaponSprite;
        }
    }
    
    // Метод для вызова из AssaultRifleWeapon
    public void FireBullet(Vector2 origin, Vector2 direction, float speed, int damage)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, origin, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Damage = damage;
            bullet.Fire(direction.normalized * speed);
        }
    }
}
