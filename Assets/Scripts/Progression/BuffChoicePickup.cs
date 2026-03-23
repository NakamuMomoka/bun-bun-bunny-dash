using UnityEngine;

/// <summary>
/// 蟾ｦ蜿ｳ2謚槭ヰ繝輔・繝励Ξ繝ｼ繧ｹ繝帙Ν繝繝ｼ縲ょ崋螳壹Ξ繝ｼ繝ｳ荳翫°繧我ｸ九∈關ｽ縺｡縲√・繝ｬ繧､繝､繝ｼ謗･隗ｦ縺ｧ蜉ｹ譫憺←逕ｨ縺励・繧｢繧堤援譁ｹ豸医☆縲・
/// </summary>
public sealed class BuffChoicePickup : MonoBehaviour
{
    public enum BuffKind
    {
        AttackSpeedUp = 0,
        MaxHpUp = 1
    }

    [SerializeField] private BuffKind buffKind;
    [SerializeField] private float attackSpeedIntervalMultiplier = 0.85f;
    [SerializeField] private int maxHpBonus = 1;
    /// <summary>Enemy 蝓ｺ貅冶誠荳九↓蟇ｾ縺吶ｋ蜑ｲ蜷茨ｼ育ｴ・85縲・0%・峨ょｮ溷柑騾溷ｺｦ縺ｯ base ﾃ・ratio ﾃ・繝輔ぉ繝ｼ繧ｺ蛟咲紫縲・/summary>
    [SerializeField] private float buffFallSpeedRatioOfEnemy = 0.8571429f;
    [SerializeField] private float enemyReferenceBaseFall = 2.8f;
    [SerializeField] private float destroyBottomY = -8f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private BuffChoicePickup _partner;
    private bool _consumed;
    private float _effectiveFallSpeed = 2.5f;

    public void Initialize(BuffKind kind, BuffChoicePickup partner, float fallSpeedMultiplier)
    {
        buffKind = kind;
        _partner = partner;
        var ratio = Mathf.Clamp(buffFallSpeedRatioOfEnemy, 0.5f, 1f);
        _effectiveFallSpeed = enemyReferenceBaseFall * ratio * Mathf.Max(0.01f, fallSpeedMultiplier);
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = kind == BuffKind.AttackSpeedUp
                ? new Color(0.35f, 1f, 0.45f, 1f)
                : new Color(0.65f, 0.55f, 1f, 1f);
        }
    }

    private void Update()
    {
        if (_consumed)
            return;
        transform.position += Vector3.down * (_effectiveFallSpeed * Time.deltaTime);
        if (transform.position.y <= destroyBottomY)
            DespawnPairMissed();
    }

    private void DespawnPairMissed()
    {
        _consumed = true;
        if (_partner != null)
        {
            var p = _partner;
            _partner = null;
            p.NotifyPartnerFallThrough();
        }
        Destroy(gameObject);
    }

    /// <summary>繝壹い逶ｸ謇九′逕ｻ髱｢螟冶誠荳九＠縺溘→縺阪√％縺｡繧峨ｒ縺ｾ縺ｨ繧√※豸医☆縲・/summary>
    public void NotifyPartnerFallThrough()
    {
        if (_consumed)
            return;
        _consumed = true;
        _partner = null;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_consumed)
            return;
        if (other.GetComponent<PlayerHealth>() == null)
            return;
        var shooter = other.GetComponent<PlayerShooter>();
        if (shooter == null)
            return;

        _consumed = true;
        var health = other.GetComponent<PlayerHealth>();
        Apply(health, shooter);
        if (_partner != null)
        {
            _partner.RemoveUnpicked();
            _partner = null;
        }
        Destroy(gameObject);
    }

    /// <summary>逶ｸ謇九′驕ｸ縺ｰ繧後◆縺ｨ縺阪∵悴驕ｸ謚槫・繧呈ｶ医☆縲・/summary>
    public void RemoveUnpicked()
    {
        if (_consumed)
            return;
        _consumed = true;
        Destroy(gameObject);
    }

    private void Apply(PlayerHealth health, PlayerShooter shooter)
    {
        switch (buffKind)
        {
            case BuffKind.AttackSpeedUp:
                shooter.ApplyAttackSpeedBuff(attackSpeedIntervalMultiplier);
                break;
            case BuffKind.MaxHpUp:
                health.ApplyMaxHpBonus(maxHpBonus);
                break;
        }
    }
}
