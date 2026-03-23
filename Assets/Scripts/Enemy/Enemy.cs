using UnityEngine;

/// <summary>
/// 下方向へ移動し、Bullet と衝突したら消える Enemy 最小実装（Arrow a Row 寄せ）。
/// </summary>
public sealed class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float destroyBottomY = -8f;

    private void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        if (transform.position.y <= destroyBottomY)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Bullet>() == null)
            return;

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
