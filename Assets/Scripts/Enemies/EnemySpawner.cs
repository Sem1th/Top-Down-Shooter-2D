using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnRadius = 12f;

    private float _timer;
    private Transform _player;
    private GameManager _gm;

    // Включаем спавнер только когда игра началась
    private void Awake() { enabled = false; }

    public void Init(Transform player, GameManager gm)
    {
        _player = player;
        _gm = gm;
        enabled = true;
        _timer = 0f;
    }

    private void Update()
    {
        if (_gm == null || _gm.IsGameOver) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnOne();
            _timer = spawnInterval;
        }
    }

    private void SpawnOne()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 pos = (_player ? _player.position : Vector3.zero) + (Vector3)(dir * spawnRadius);

        var e = Instantiate(enemyPrefab, pos, Quaternion.identity);
        e.Init(_player, _gm);
    }
}
