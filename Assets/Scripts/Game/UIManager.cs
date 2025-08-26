using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI survivalTimeText;

    private GameManager _gm;

    private void Awake()
    {
        _gm = FindObjectOfType<GameManager>();
        ServiceLocator.Register<UIManager>(this);
    }

    private void Start()
    {
        ShowStartMenu();
    }

    public void UpdateHUD(int lives, int score, float time)
    {
        if (!hudPanel) return;
        if (livesText) livesText.text = $"Жизни: {lives}";
        if (scoreText) scoreText.text = $"Счет: {score}";
        if (timeText)  timeText.text  = $"Время: {time:F1}";
    }

    public void ShowStartMenu()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (hudPanel)      hudPanel.SetActive(false);
        if (pausePanel)    pausePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    public void ShowHUD()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (hudPanel)      hudPanel.SetActive(true);
        if (pausePanel)    pausePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
    }

    public void ShowPause()
    {
        if (pausePanel)    pausePanel.SetActive(true);
        if (hudPanel)      hudPanel.SetActive(false);
    }

    public void ShowGameOver(int score, float time)
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
        if (hudPanel)      hudPanel.SetActive(false);
        if (finalScoreText)    finalScoreText.text = $"Итоговый счет: {score}";
        if (survivalTimeText)  survivalTimeText.text = $"Время выживания: {time:F1}";
    }

    // ==== UI Buttons ====
    public void UI_StartGame() => _gm?.StartGame();
    public void UI_Pause()     => _gm?.Pause();
    public void UI_Resume()    => _gm?.Resume();
    public void UI_Restart()   => _gm?.Restart();
    public void UI_Menu()      => _gm?.BackToMenu();
}
