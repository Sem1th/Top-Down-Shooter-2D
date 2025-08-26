using UnityEngine;


public class ProjectileWeapon : IWeapon
{
    
    [SerializeField] private float BulletSpeed = 16f;
    public float FireRate { get; set; } = 1f;
    public int Damage { get; set; } = 1;
    public Sprite WeaponSprite { get; set; }
    
    [Header("Burst Settings")]
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstDelay = 0.1f;
    [SerializeField] private float spreadAngle = 5f; 
    
    private float _cooldown;
    private float _burstTimer;
    private int _burstCounter;
    private bool _isBursting;
    private GameObject _bulletPrefab;
    private PlayerShooting _shootingSystem;
    private Vector2 _currentOrigin;
    private Vector2 _currentDirection;

    public ProjectileWeapon(GameObject bulletPrefab, Sprite sprite)
    {
        _bulletPrefab = bulletPrefab;
        WeaponSprite = sprite;
    }

    public void SetShootingSystem(PlayerShooting shootingSystem)
    {
        _shootingSystem = shootingSystem;
    }

    public void Tick(float dt)
    {
        if (_cooldown > 0)
            _cooldown -= dt;

        if (_isBursting)
        {
            _burstTimer -= dt;
            if (_burstTimer <= 0 && _burstCounter < burstCount)
            {
                FireSingleBullet();
                _burstCounter++;
                _burstTimer = burstDelay;

                if (_burstCounter >= burstCount)
                {
                    _isBursting = false;
                    _cooldown = 1f / Mathf.Max(0.01f, FireRate); // устанавливаем перезарядку после серии
                }
            }
        }
    }

    public bool TryFire(Vector2 origin, Vector2 dir)
    {
        if (_cooldown > 0 || _isBursting) return false;

        _isBursting = true;
        _burstCounter = 0;
        _burstTimer = 0f;

        _currentOrigin = origin;
        _currentDirection = dir;

        return true;
    }

    private void FireSingleBullet()
    {
        if (_shootingSystem == null) return;

        // Добавляем небольшой разброс
        float angleOffset = Random.Range(-spreadAngle, spreadAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, angleOffset) * _currentDirection;

        _shootingSystem.FireBullet(_currentOrigin, spreadDir, BulletSpeed, Damage);
    }
}
