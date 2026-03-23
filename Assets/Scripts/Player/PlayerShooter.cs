using UnityEngine;

/// <summary>
/// 一定間隔で上方向へ弾を自動発射する（Arrow a Row 寄せ）。
/// </summary>
public sealed class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float fireInterval = 0.35f;
    [SerializeField] private float minFireInterval = 0.08f;
    [SerializeField] private Vector2 spawnOffset = new Vector2(0f, 0.5f);

    private float _cooldown;

    /// <summary>攻撃速度アップ（射撃間隔を掛け算で短縮）。</summary>
    public void ApplyAttackSpeedBuff(float intervalMultiplier)
    {
        fireInterval = Mathf.Max(minFireInterval, fireInterval * intervalMultiplier);
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;
        if (_cooldown > 0f)
            return;
        _cooldown = fireInterval;
        Fire();
    }

    private void Fire()
    {
        if (bulletPrefab == null)
            return;
        var position = transform.position + (Vector3)spawnOffset;
        var bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        bullet.Initialize(Vector3.up);
    }
}
