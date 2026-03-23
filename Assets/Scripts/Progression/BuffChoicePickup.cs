using UnityEngine;

/// <summary>
/// 左右2択バフのプレースホルダー。固定レーン上から下へ落ち、プレイヤー接触で効果適用しペアを片方消す。
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
    [SerializeField] private float fallSpeed = 2.5f;
    [SerializeField] private float destroyBottomY = -8f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private BuffChoicePickup _partner;
    private bool _consumed;

    public void Initialize(BuffKind kind, BuffChoicePickup partner)
    {
        buffKind = kind;
        _partner = partner;
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
        transform.position += Vector3.down * (fallSpeed * Time.deltaTime);
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

    /// <summary>ペア相手が画面外落下したとき、こちらをまとめて消す。</summary>
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

    /// <summary>相手が選ばれたとき、未選択側を消す。</summary>
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
