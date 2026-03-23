using UnityEngine;

/// <summary>
/// 直進する弾。寿命で消滅（最小実装）。
/// </summary>
public sealed class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 12f;
    [SerializeField] private float maxLifetime = 3f;

    private float _elapsed;
    private Vector3 _direction = Vector3.right;

    public void Initialize(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.0001f)
            _direction = direction.normalized;
    }

    private void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
        _elapsed += Time.deltaTime;
        if (_elapsed >= maxLifetime)
            Destroy(gameObject);
    }
}
