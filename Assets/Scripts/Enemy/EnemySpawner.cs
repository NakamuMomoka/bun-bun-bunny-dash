using UnityEngine;

/// <summary>
/// 右側から一定間隔で Enemy を出現させる最小スポナー。
/// </summary>
public sealed class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnX = 9f;
    [SerializeField] private Vector2 spawnYRange = new Vector2(-3.5f, 3.5f);

    private float _cooldown;

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

        float spawnY = Random.Range(spawnYRange.x, spawnYRange.y);
        var position = new Vector3(spawnX, spawnY, 0f);
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
