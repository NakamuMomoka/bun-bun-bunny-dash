using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// HP 0 時に ResultScene へ遷移する最小ハンドラ。
/// </summary>
public sealed class GameOverHandler : MonoBehaviour
{
    [SerializeField] private string resultSceneName = "ResultScene";
    private bool _isGameOver;

    public void TriggerGameOver()
    {
        if (_isGameOver)
            return;

        _isGameOver = true;
        SceneManager.LoadScene(resultSceneName);
    }
}
