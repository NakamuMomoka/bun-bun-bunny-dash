# AGENTS.md

このファイルは Bun Bun Bunny Dash の Unity 2D 開発における AI エージェント向け共通ルールです。

## 基本方針

- 常に最小スコープで対応する
- 既存責務を壊さない
- 無関係な変更を混ぜない
- 実装したら必ず Unity Console を確認する
- エラーがあれば修正して再確認する
- 変更したファイル一覧を最後に報告する
- Scene / Prefab / Script の責務を分ける
- 依頼がMVP範囲を超える場合は分割案を先に出す

## Unity 実装ルール

- Script は `Assets/Scripts/` 配下の責務別フォルダへ配置する
- 1クラス1責務を原則とする
- Inspector で調整する値は `[SerializeField] private` を優先する
- Prefab 参照をコード内にハードコードしない
- `FindObjectOfType` の多用を避ける
- Player / Enemy / UI / GameManager の責務を混ぜない
- ScriptableObject で表現できる設定値は ScriptableObject を検討する
- MVP段階では過度な最適化や抽象化をしない
- 依頼がない限り、新規パッケージ追加をしない

## 進行中のゲーム方針

- 目標は Arrow a Row 風の超小型MVP
- コア体験は「横移動」「自動攻撃」「敵撃破」「レベルアップ3択」
- ドット絵美少女の見た目は歓迎だが、まずゲームループ完成を優先する
- 恒久強化、転生、課金、装備などは MVP 完成後に検討する

## テスト / 確認

- 変更後は必ず Unity Console の error / warning を確認する
- 可能なら EditMode または PlayMode テストを追加する
- テストが難しい場合は、手動確認手順を明記する
- シーンやプレハブを変更した場合は、その影響範囲を明示する

## 禁止事項

- 依頼にない大規模リファクタ
- 依頼にないアーキテクチャ変更
- 依頼にないパッケージ追加
- UI演出やアート品質の作り込みをMVP前に始めること
- placeholder 素材の差し替えをゲームロジック完成前に優先すること
- 既存 Scene / Prefab の破壊的変更を黙って行うこと

## 変更報告フォーマット

- 目的
- 変更点
- 影響範囲
- Console確認結果
- テスト結果
- 未解決リスク
