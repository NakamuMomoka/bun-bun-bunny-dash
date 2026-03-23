# Assets ディレクトリ方針

この ZIP は Unity プロジェクトに上書き配置する前提です。
Scene / Prefab / Sprite / Audio などの Unity 実体は、Unity Editor 上で作成してください。

## フォルダ構成（責務別）

- `Scenes/` … `TitleScene`, `GameScene`, `ResultScene`（`docs/mvp-spec.md` / `docs/unity-setup.md` 参照）
- `Scripts/Core`, `Player`, `Enemy`, `Combat`, `Progression`, `UI`, `Data` … スクリプトの置き場所
- `Prefabs/Characters`, `Enemies`, `Projectiles`, `UI` … プレハブ
- `Art/Backgrounds`, `Characters`, `Enemies`, `UI` … スプライト・画像素材
- `Audio/` … 音声
- `Settings/` … ScriptableObject やプロジェクト設定アセット（必要に応じて）
- `Tests/EditMode`, `PlayMode` … テストアセンブリ用（`Assets/Tests` 配下のテストコード）

## 推奨作成順

1. `Assets/Scenes/TitleScene.unity`
2. `Assets/Scenes/GameScene.unity`
3. `Assets/Scenes/ResultScene.unity`
4. `Assets/Prefabs/Characters/Player.prefab`
5. `Assets/Prefabs/Enemies/Enemy.prefab`
6. `Assets/Prefabs/Projectiles/Bullet.prefab`

## ドット絵方針

- 最初は仮の四角形スプライトで実装してOK
- プレイヤーだけ先にドット絵美少女へ差し替える
- その後に敵、背景、UIの順で整える
