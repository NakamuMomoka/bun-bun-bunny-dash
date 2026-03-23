using System;
using UnityEngine;
using UObject = UnityEngine.Object;

/// <summary>
/// Player の最小 HP 管理。Enemy 接触でダメージを受ける。
/// </summary>
public sealed class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 30;
    [SerializeField] private GameOverHandler gameOverHandler;
    [SerializeField] private Transform hpAnchor;
    [SerializeField] private Vector3 hpLabelOffset = new Vector3(0f, 0.9f, 0f);
    [SerializeField] private float hitColliderRadius = 0.35f;
    [SerializeField] private float bodyHitDistance = 1.05f;
    [SerializeField] private float hitCooldownSeconds = 0.2f;

    private int _currentHp;
    private GUIStyle _hpGuiStyle;
    private GUIStyle _hpShadowStyle;
    private float _nextAllowedHitTime;

    public int CurrentHp => _currentHp;
    public int MaxHp => maxHp;
    public event Action<int, int> HpChanged;

    private void Awake()
    {
        if (hpAnchor == null)
        {
            var penguin = transform.Find("PenguinModel");
            if (penguin != null)
                hpAnchor = penguin;
        }

        // Keep runtime HP baseline stable even if old scene-serialized value remains.
        maxHp = Mathf.Max(30, maxHp);
        Ensure2DHitComponents();
        _currentHp = Mathf.Max(1, maxHp);
        NotifyHpChanged();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null)
            return;
        if (other.GetComponent<Bullet>() != null)
            return;
        if (Time.time < _nextAllowedHitTime)
            return;

        var delta = enemy.transform.position - transform.position;
        var sqrDistanceXZ = (delta.x * delta.x) + (delta.z * delta.z);
        if (sqrDistanceXZ > bodyHitDistance * bodyHitDistance)
            return;

        ApplyEnemyHit(enemy.gameObject);
    }

    private void Update()
    {
        if (Time.time < _nextAllowedHitTime)
            return;

        // Fallback hit check: in mixed 2D/3D view setups trigger callbacks can occasionally miss.
        var enemies = UObject.FindObjectsByType<Enemy>(FindObjectsInactive.Exclude);
        var maxSqr = bodyHitDistance * bodyHitDistance;
        for (var i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];
            if (enemy == null)
                continue;

            var delta = enemy.transform.position - transform.position;
            var sqrDistanceXZ = (delta.x * delta.x) + (delta.z * delta.z);
            if (sqrDistanceXZ > maxSqr)
                continue;

            ApplyEnemyHit(enemy.gameObject);
            break;
        }
    }

    private void Ensure2DHitComponents()
    {
        var col = GetComponent<CircleCollider2D>();
        if (col == null)
            col = gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        if (col.radius < hitColliderRadius)
            col.radius = hitColliderRadius;

        var rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void ApplyEnemyHit(GameObject enemyObject)
    {
        _nextAllowedHitTime = Time.time + Mathf.Max(0.01f, hitCooldownSeconds);
        Damage(1);
        if (enemyObject != null)
            Destroy(enemyObject);
    }

    private void Damage(int amount)
    {
        _currentHp -= Mathf.Max(1, amount);
        _currentHp = Mathf.Max(0, _currentHp);
        NotifyHpChanged();

        if (_currentHp > 0)
            return;

        if (gameOverHandler != null)
            gameOverHandler.TriggerGameOver();
    }

    /// <summary>最大HPアップ（現在HPも同分回復）。</summary>
    public void ApplyMaxHpBonus(int bonus)
    {
        var b = Mathf.Max(1, bonus);
        maxHp += b;
        _currentHp += b;
        NotifyHpChanged();
    }

    private void NotifyHpChanged()
    {
        HpChanged?.Invoke(_currentHp, maxHp);
    }

    private void OnGUI()
    {
        var cam = Camera.main;
        if (cam == null)
            return;

        var anchorPosition = hpAnchor != null ? hpAnchor.position : transform.position;
        var world = anchorPosition + hpLabelOffset;
        var viewport = cam.WorldToViewportPoint(world);
        if (viewport.z <= 0f)
            return;
        if (viewport.x < 0f || viewport.x > 1f || viewport.y < 0f || viewport.y > 1f)
            return;

        if (_hpGuiStyle == null)
        {
            _hpGuiStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };

            _hpShadowStyle = new GUIStyle(_hpGuiStyle)
            {
                normal = { textColor = new Color(0f, 0f, 0f, 0.9f) }
            };
        }

        var text = $"HP {_currentHp}/{maxHp}";
        var width = 128f;
        var height = 28f;
        var x = viewport.x * Screen.width - (width * 0.5f);
        var y = (1f - viewport.y) * Screen.height - height;
        var rect = new Rect(x, y, width, height);

        // Draw a tiny outline to keep readability on bright backgrounds.
        GUI.Label(new Rect(rect.x - 1f, rect.y, rect.width, rect.height), text, _hpShadowStyle);
        GUI.Label(new Rect(rect.x + 1f, rect.y, rect.width, rect.height), text, _hpShadowStyle);
        GUI.Label(new Rect(rect.x, rect.y - 1f, rect.width, rect.height), text, _hpShadowStyle);
        GUI.Label(new Rect(rect.x, rect.y + 1f, rect.width, rect.height), text, _hpShadowStyle);
        GUI.Label(rect, text, _hpGuiStyle);
    }
}
