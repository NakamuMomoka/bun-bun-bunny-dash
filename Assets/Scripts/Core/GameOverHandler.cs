using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// HP 0 時に ResultScene へ遷移する最小ハンドラ。
/// 物理コールバック直後の同期 LoadScene はエディタでフリーズしやすいため、1 フレーム遅延＋非同期読み込みにする。
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
        StartCoroutine(LoadResultSceneDeferred());
    }

    private IEnumerator LoadResultSceneDeferred()
    {
        yield return null;

        var op = SceneManager.LoadSceneAsync(resultSceneName, LoadSceneMode.Single);
        if (op == null)
            yield break;

        yield return op;
    }
}
