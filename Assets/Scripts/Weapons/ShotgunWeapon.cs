using UnityEngine;


public class ShotgunWeapon : IWeapon
{
    [SerializeField] private float BulletSpeed = 18f;
    [SerializeField] private float Accuracy = 0.95f;
    public float FireRate { get; set; } = 2.5f;
    public int Damage { get; set; } = 1; 
    public Sprite WeaponSprite { get; set; }
     

    private float _cooldown;
    private GameObject _bulletPrefab;

    public ShotgunWeapon(GameObject bulletPrefab, Sprite sprite)
    {
        _bulletPrefab = bulletPrefab;
        WeaponSprite = sprite;
    }

    public void Tick(float dt)
    {
        if (_cooldown > 0) _cooldown -= dt;
    }

    public bool TryFire(Vector2 origin, Vector2 dir)
    {
        if (_cooldown > 0) return false;
        _cooldown = 1f / Mathf.Max(0.01f, FireRate);

        // Добавляем небольшую случайность для реализма
        Vector2 accurateDir = dir.normalized;
        if (Accuracy < 1f)
        {
            float inaccuracy = (1f - Accuracy) * 5f;
            accurateDir = Quaternion.Euler(0, 0, Random.Range(-inaccuracy, inaccuracy)) * accurateDir;
        }

        GameObject bulletObj = GameObject.Instantiate(_bulletPrefab, origin, Quaternion.identity);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Damage = Damage;
            bullet.Fire(accurateDir * BulletSpeed);
        }

        return true;
    }
}
