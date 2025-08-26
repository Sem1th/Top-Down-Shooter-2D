using UnityEngine;

[System.Serializable]
public class WeaponSystem : IWeapon
{
    [SerializeField] private float BulletSpeed = 16f;
    public float FireRate { get; set; } = 8f;
    public int Damage { get; set; } = 1;
    public Sprite WeaponSprite { get; set; }
    
    
    [Header("Burst Settings")]
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstDelay = 0.1f;
    
    private float _cooldown;
    private float _burstTimer;
    private int _burstCounter;
    private bool _isBursting;
    
    private Vector2 _currentOrigin;
    private Vector2 _currentDirection;
    
    private GameObject _bulletPrefab;
    private PlayerShooting _shootingSystem;

    public WeaponSystem(GameObject bulletPrefab, Sprite sprite)
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
        if (_cooldown > 0) _cooldown -= dt;
        
        if (_isBursting)
        {
            _burstTimer -= dt;
            if (_burstTimer <= 0 && _burstCounter < burstCount)
            {
                FireSingleBullet();
                _burstTimer = burstDelay;
                _burstCounter++;
                
                if (_burstCounter >= burstCount)
                {
                    _isBursting = false;
                }
            }
        }
    }

    public bool TryFire(Vector2 origin, Vector2 dir)
    {
        if (_cooldown > 0 || _isBursting) return false;
        
        _cooldown = 1f / Mathf.Max(0.01f, FireRate);
        _isBursting = true;
        _burstCounter = 0;
        _burstTimer = 0f;
        
        _currentOrigin = origin;
        _currentDirection = dir;
        
        // Сразу выпускаем первую пулю
        FireSingleBullet();
        _burstTimer = burstDelay;
        _burstCounter++;
        
        return true;
    }
    
    private void FireSingleBullet()
    {
        if (_shootingSystem != null)
        {
            _shootingSystem.FireBullet(_currentOrigin, _currentDirection, BulletSpeed, Damage);
        }
    }
}
