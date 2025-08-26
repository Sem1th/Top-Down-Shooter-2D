using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint; 

    [Header("Systems")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private UIManager uiManager;

    [Header("Gameplay")]
    [SerializeField] private int startingLives = 3;

    public int PlayerLives { get; private set; }
    public int Score { get; private set; }
    public float SurvivalTime { get; private set; }
    public bool IsGameOver { get; private set; }

    private GameObject _playerGO;

    private void Awake()
    {
        ServiceLocator.Register<GameManager>(this);
        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        if (enemySpawner == null) enemySpawner = FindObjectOfType<EnemySpawner>();

        Time.timeScale = 0f;          // пауза до старта
        IsGameOver = false;
        uiManager?.ShowStartMenu();
    }

    private void Update()
    {
        if (!IsGameOver && _playerGO != null)
        {
            SurvivalTime += Time.deltaTime;
            uiManager?.UpdateHUD(PlayerLives, Score, SurvivalTime);
        }
    }

    // ==== Game Flow ====
    public void StartGame()
    {
        ResetSession();

        Vector3 spawnPos = playerSpawnPoint ? playerSpawnPoint.position : Vector3.zero;

        if (_playerGO == null)
            _playerGO = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        else
        {
            _playerGO.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
            _playerGO.SetActive(true);
        }

        enemySpawner?.Init(_playerGO.transform, this);

        Time.timeScale = 1f;
        uiManager?.ShowHUD();
    }

    public void Pause()
    {
        if (IsGameOver) return;
        Time.timeScale = 0f;
        uiManager?.ShowPause();
    }

    public void Resume()
    {
        if (IsGameOver) return;
        Time.timeScale = 1f;
        uiManager?.ShowHUD();
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Time.timeScale = 0f;

        if (_playerGO) _playerGO.SetActive(false);
        uiManager?.ShowGameOver(Score, SurvivalTime);
    }

    public void Restart() =>
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void BackToMenu() =>
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    // ==== State ====
    public void AddScore(int value)
    {
        Score += value;
        uiManager?.UpdateHUD(PlayerLives, Score, SurvivalTime);
    }

    public void DamagePlayer(int damage)
    {
        if (IsGameOver) return;

        PlayerLives -= damage;
        uiManager?.UpdateHUD(PlayerLives, Score, SurvivalTime);

        if (PlayerLives <= 0)
            GameOver();
    }

    private void ResetSession()
    {
        PlayerLives = startingLives;
        Score = 0;
        SurvivalTime = 0f;
        IsGameOver = false;
    }
}