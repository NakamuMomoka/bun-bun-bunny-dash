using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Issue #3: GameScene に移動・自動射撃を配線。Unity で Tools から 1 回実行する。
/// </summary>
public static class BunBunWireGameScene
{
    private const string MenuPath = "Tools/BunBun/Wire GameScene (Issue 3)";
    private const string GameScenePath = "Assets/Scenes/GameScene.unity";
    private const string TitleScenePath = "Assets/Scenes/TitleScene.unity";

    [MenuItem(MenuPath)]
    public static void WireGameScene()
    {
        var tex = Texture2D.whiteTexture;
        Sprite MakeSprite()
        {
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
        }

        var projDir = "Assets/Prefabs/Projectiles";
        if (!Directory.Exists(projDir))
            Directory.CreateDirectory(projDir);

        var bulletGo = new GameObject("Bullet");
        var bsr = bulletGo.AddComponent<SpriteRenderer>();
        bsr.sprite = MakeSprite();
        bsr.color = new Color(1f, 0.92f, 0.2f, 1f);
        bulletGo.AddComponent<Bullet>();
        var prefabPath = projDir + "/Bullet.prefab";
        var prefabRoot = PrefabUtility.SaveAsPrefabAsset(bulletGo, prefabPath);
        Object.DestroyImmediate(bulletGo);
        var bulletOnPrefab = prefabRoot.GetComponent<Bullet>();

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        var player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        var psr = player.AddComponent<SpriteRenderer>();
        psr.sprite = MakeSprite();
        psr.color = new Color(0.35f, 0.9f, 1f, 1f);
        player.AddComponent<PlayerMovement>();
        var shooter = player.AddComponent<PlayerShooter>();
        var so = new SerializedObject(shooter);
        var prop = so.FindProperty("bulletPrefab");
        if (prop != null)
        {
            prop.objectReferenceValue = bulletOnPrefab;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        EditorSceneManager.SaveScene(scene, GameScenePath);

        EnsureGameSceneInBuildSettings();

        AssetDatabase.SaveAssets();
        Debug.Log("[BunBun] Wired GameScene (Player movement + shooter), Bullet prefab. Build Settings: GameScene updated without removing other scenes.");
    }

    /// <summary>
    /// GameScene のみ有効化・更新し、TitleScene / ResultScene など既存の Build Settings エントリは維持する。
    /// </summary>
    private static void EnsureGameSceneInBuildSettings()
    {
        var list = new List<EditorBuildSettingsScene>();
        if (EditorBuildSettings.scenes != null)
            list.AddRange(EditorBuildSettings.scenes);

        var gameEntry = new EditorBuildSettingsScene(GameScenePath, true);
        var index = list.FindIndex(s => s.path == GameScenePath);
        if (index >= 0)
            list[index] = gameEntry;
        else
        {
            var afterTitle = list.FindIndex(s => s.path == TitleScenePath);
            if (afterTitle >= 0)
                list.Insert(afterTitle + 1, gameEntry);
            else
                list.Add(gameEntry);
        }

        EditorBuildSettings.scenes = list.ToArray();
    }
}
