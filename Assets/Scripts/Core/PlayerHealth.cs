using UnityEngine;

/// <summary>
/// Player の最小 HP 管理。Enemy 接触でダメージを受ける。
/// </summary>
public sealed class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 3;
    [SerializeField] private GameOverHandler gameOverHandler;

    private int _currentHp;

    private void Awake()
    {
        _currentHp = Mathf.Max(1, maxHp);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() == null)
            return;

        Damage(1);
        Destroy(other.gameObject);
    }

    private void Damage(int amount)
    {
        _currentHp -= Mathf.Max(1, amount);
        if (_currentHp > 0)
            return;

        if (gameOverHandler != null)
            gameOverHandler.TriggerGameOver();
    }
}
