using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    private InputActions controls;
    public bool IsGameOver;

    void Awake()
    {
        controls = new InputActions();
        controls.Player.Pause.performed += ctx => Pause();
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    public void Pause()
    {
        if (IsGameOver) return;
        Time.timeScale = 0f;
        uiManager?.ShowPause();
    }
}
