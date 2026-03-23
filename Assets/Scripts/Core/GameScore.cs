using UnityEngine;

/// <summary>
/// ゲーム中スコアの最小保持（敵撃破などで加算）。
/// </summary>
public sealed class GameScore : MonoBehaviour
{
    [SerializeField, Min(0)] private int _score;
    [SerializeField] private Vector2 screenOffset = new Vector2(18f, 14f);

    private GUIStyle _scoreGuiStyle;
    private GUIStyle _scoreShadowStyle;

    public int CurrentScore => _score;

    public void AddScore(int delta)
    {
        if (delta <= 0)
            return;
        _score += delta;
    }

    private void OnGUI()
    {
        if (_scoreGuiStyle == null)
        {
            _scoreGuiStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 28,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperLeft,
                normal = { textColor = Color.white }
            };

            _scoreShadowStyle = new GUIStyle(_scoreGuiStyle)
            {
                normal = { textColor = new Color(0f, 0f, 0f, 0.92f) }
            };
        }

        var text = $"SCORE {_score}";
        var rect = new Rect(screenOffset.x, screenOffset.y, 260f, 40f);

        // Tiny outline to keep readability on bright sky/ground.
        GUI.Label(new Rect(rect.x - 1f, rect.y, rect.width, rect.height), text, _scoreShadowStyle);
        GUI.Label(new Rect(rect.x + 1f, rect.y, rect.width, rect.height), text, _scoreShadowStyle);
        GUI.Label(new Rect(rect.x, rect.y - 1f, rect.width, rect.height), text, _scoreShadowStyle);
        GUI.Label(new Rect(rect.x, rect.y + 1f, rect.width, rect.height), text, _scoreShadowStyle);
        GUI.Label(rect, text, _scoreGuiStyle);
    }
}
