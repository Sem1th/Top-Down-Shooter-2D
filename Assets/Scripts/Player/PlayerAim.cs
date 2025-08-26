using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform gunSprite;
    [SerializeField] private Transform firePoint;
    private Transform _player;
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
        _player = transform.root;
    }

    private void Update()
    {   
        if (Time.timeScale == 0f) 
        return; // Игра на паузе — не крутим спрайт

        // Мировая позиция мыши
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = _mainCam.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        // Направление
        Vector2 aimDir = (worldPos - firePoint.position).normalized;

        // Угол для поворота
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        gunSprite.rotation = Quaternion.Euler(0f, 0f, angle);

        // Флип спрайта, если мышка слева
        if (worldPos.x < _player.position.x)
        {
            gunSprite.localScale = new Vector3(0.35f, -0.35f, 0.35f); // переворачиваем по Y
        }
        else
        {
            gunSprite.localScale = new Vector3(0.35f, 0.35f, 0.35f);  // нормальное положение
        }
    }
}
