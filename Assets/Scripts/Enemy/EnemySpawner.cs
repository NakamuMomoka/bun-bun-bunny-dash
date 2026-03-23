using UnityEngine;

/// <summary>
/// 固定2レーンの上側から一定間隔で Enemy を出現。スコアフェーズに応じて必要弾数・落下速度を決定。
/// </summary>
public sealed class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnY = -3.3f;
    [SerializeField] private float spawnFarZ = 9.5f;
    [SerializeField] private bool usePlayerBulletSpawnAsBase = true;
    [SerializeField] private float spawnAheadZFromBullet = 8.7f;
    [SerializeField] private float laneLeftX = -2f;
    [SerializeField] private float laneRightX = 2f;
    [SerializeField] private GameScore gameScore;
    [SerializeField] private int scorePerEnemyKill = 10;

    private float _cooldown;
    private PlayerShooter _playerShooter;

    private void Awake()
    {
        _playerShooter = FindFirstObjectByType<PlayerShooter>();
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        if (_cooldown > 0f)
            return;

        _cooldown = spawnInterval;
        Spawn();
    }

    private void Spawn()
    {
        if (enemyPrefab == null)
            return;

        float spawnX = Random.value >= 0.5f ? laneLeftX : laneRightX;
        var baseY = spawnY;
        var baseZ = spawnFarZ;
        if (usePlayerBulletSpawnAsBase && _playerShooter != null)
        {
            var bulletSpawn = _playerShooter.GetSpawnPosition();
            baseY = bulletSpawn.y;
            baseZ = bulletSpawn.z + spawnAheadZFromBullet;
        }

        var position = new Vector3(spawnX, baseY, baseZ);
        var instance = Instantiate(enemyPrefab, position, Quaternion.identity);
        var score = gameScore != null ? gameScore.CurrentScore : 0;
        var hits = ScorePhaseScaling.RollEnemyRequiredHits(score);
        var mult = ScorePhaseScaling.GetFallSpeedMultiplier(score);
        var enemy = instance.GetComponent<Enemy>();
        if (enemy != null)
            enemy.Initialize(hits, gameScore, scorePerEnemyKill, mult);
    }
}
