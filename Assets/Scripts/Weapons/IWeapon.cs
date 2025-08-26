using UnityEngine;


public interface IWeapon
{
    float FireRate { get; set; }
    int Damage { get; set; }
    Sprite WeaponSprite { get; set; }
    void Tick(float dt);
    bool TryFire(Vector2 origin, Vector2 dir);
}
