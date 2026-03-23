using UnityEngine;

/// <summary>
/// 画面上側から一定間隔で Enemy を出現させる最小スポナー（Arrow a Row 寄せ）。
/// </summary>
public sealed class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private float spawnInterval = 1.2f;
    [SerializeField] private float spawnTopY = 5.5f;
    [SerializeField] private Vector2 spawnXRange = new Vector2(-4f, 4f);

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

        float spawnX = Random.Range(spawnXRange.x, spawnXRange.y);
        var position = new Vector3(spawnX, spawnTopY, 0f);
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
