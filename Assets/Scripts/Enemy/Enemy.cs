using UnityEngine;

/// <summary>
/// Moves downward and dies when HP reaches zero from Bullet damage.
/// Shows remaining hit count above the enemy.
/// </summary>
public sealed class Enemy : MonoBehaviour
{
    [SerializeField] private float baseMoveSpeed = 2.8f;
    [SerializeField] private float destroyNearZ = -8f;
    [SerializeField] private Vector2 labelOffset = new Vector2(0f, 0.45f);

    private float _moveSpeed = 2.8f;
    private int _currentHp = 1;
    private int _maxHp = 1;
    private GameScore _gameScore;
    private int _killScore;
    private TextMesh _hitsLabel;
    /// <summary>Set spawn HP, kill score, and fall speed multiplier.</summary>
    public void Initialize(int hp, GameScore gameScore, int killScore, float fallSpeedMultiplier)
    {
        _maxHp = Mathf.Max(1, hp);
        _currentHp = _maxHp;
        _gameScore = gameScore;
        _killScore = Mathf.Max(0, killScore);
        _moveSpeed = baseMoveSpeed * Mathf.Max(0.01f, fallSpeedMultiplier);
        EnsureLabel();
        UpdateHitsLabel();
    }

    private void EnsureLabel()
    {
        if (_hitsLabel != null)
            return;

        var go = new GameObject("HitsLabel");
        go.transform.SetParent(transform, false);
        go.transform.localPosition = new Vector3(labelOffset.x, labelOffset.y, 0f);

        _hitsLabel = go.AddComponent<TextMesh>();
        _hitsLabel.anchor = TextAnchor.MiddleCenter;
        _hitsLabel.alignment = TextAlignment.Center;
        _hitsLabel.fontSize = 56;
        _hitsLabel.characterSize = 0.045f;
        _hitsLabel.fontStyle = FontStyle.Bold;
        _hitsLabel.color = Color.black;
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font != null)
            _hitsLabel.font = font;

        var mr = go.GetComponent<MeshRenderer>();
        if (mr != null)
            mr.sortingOrder = 20;
    }

    private void UpdateHitsLabel()
    {
        if (_hitsLabel != null)
            _hitsLabel.text = _currentHp.ToString();
    }

    private void Update()
    {
        transform.position += Vector3.back * (_moveSpeed * Time.deltaTime);
        if (transform.position.z <= destroyNearZ)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var bullet = other.GetComponent<Bullet>();
        if (bullet == null)
            return;

        Destroy(other.gameObject);
        _currentHp -= Mathf.Max(1, bullet.Damage);
        if (_currentHp <= 0)
        {
            if (_gameScore != null && _killScore > 0)
                _gameScore.AddScore(_killScore);
            Destroy(gameObject);
            return;
        }

        UpdateHitsLabel();
    }

}
