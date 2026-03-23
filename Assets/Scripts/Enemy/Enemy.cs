using UnityEngine;

/// <summary>
/// 左方向へ移動し、Bullet と衝突したら消える Enemy 最小実装。
/// </summary>
public sealed class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float destroyX = -12f;

    private void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        if (transform.position.x <= destroyX)
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
