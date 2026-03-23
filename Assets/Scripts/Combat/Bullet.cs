using UnityEngine;

/// <summary>
/// 逶ｴ騾ｲ縺吶ｋ蠑ｾ縲ょｯｿ蜻ｽ縺ｧ豸域ｻ・ｼ域怙蟆丞ｮ溯｣・ｼ峨・
/// </summary>
public sealed class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 16f;
    [SerializeField] private float maxLifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float hitRadius = 0.24f;

    public int Damage => damage;

    private float _elapsed;
    private Vector3 _direction = Vector3.up;

    private void Awake()
    {
        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            col.isTrigger = true;
            col.radius = Mathf.Max(col.radius, hitRadius);
        }

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

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
