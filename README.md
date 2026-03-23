# Bun Bun Bunny Dash

2Dドット絵美少女が走りながら敵を迎撃する、Unity 2D 向けの小規模アクションMVPです。

このリポジトリは **ゲーム本体の完成版ではなく、Unity 2D プロジェクトに重ねる AI 開発運用スターター** です。
元の `ai-dev-project-template` の考え方をベースに、Unity / Cursor / Issue 駆動向けへ調整しています。

## 目標

- Arrow a Row 風の「横移動 + 自動攻撃 + レベルアップ3択」のコア体験を作る
- まずは 1 プレイ完結の MVP を作る
- Cursor と AI エージェントが暴走しにくい運用ルールを整える
- あとからドット絵美少女や演出を足しやすい形にする

## MVP スコープ

### 入れるもの
- プレイヤーの左右移動
- 自動射撃
- 敵の定期スポーン
- 被弾と HP
- 敵撃破で経験値獲得
- レベルアップ時の3択強化
- タイトル / ゲーム / リザルトの3画面

### 今回は入れないもの
- 恒久強化
- 転生
- 装備
- ガチャ
- ストーリーイベント
- 複数キャラ
- オンライン要素
- 本格的な課金設計

## 推奨の進め方

1. Unity Hub で **新規 2D Core プロジェクト** を作る（**または**、リポジトリに `ProjectSettings/` が含まれる場合は Hub で **このリポジトリのルートを追加** して開く。二重の Unity プロジェクトフォルダは置かない）
2. 新規作成した場合は、その Unity プロジェクトのルートへ、この ZIP の中身を上書き配置する
3. Git 初期化または GitHub リポジトリへ push する
4. Cursor で開いて `.cursor/rules` と `AGENTS.md` を読ませる
5. `docs/issue-backlog.md` の Issue から順番に実装する（詳細は `docs/unity-setup.md` の「0. リポジトリルートと Unity プロジェクト」も参照）

## 最初にやること

- `project-config.json` の `__OWNER__` を自分の GitHub アカウントに置き換える
- `custom-gpt/openapi.json` と `custom-gpt/instructions.md` の `__OWNER__` を置き換える
- Unity で `Assets/Scenes/TitleScene.unity`, `GameScene.unity`, `ResultScene.unity` を作成する（命名・役割は `docs/unity-setup.md` の「シーン作成の方針」に合わせる）
- Cursor に `docs/cursor-workflow.md` の文面で最初のセットアップ Issue を実装させる

## 想定ディレクトリ

- `Assets/Scripts/Core`: ゲーム全体制御
- `Assets/Scripts/Player`: プレイヤー挙動
- `Assets/Scripts/Enemy`: 敵挙動
- `Assets/Scripts/Combat`: 弾やダメージ
- `Assets/Scripts/Progression`: 経験値 / レベルアップ
- `Assets/Scripts/UI`: HUD や各画面
- `Assets/Scripts/Data`: ScriptableObject などの設定データ

## 備考

このスターターは、**Unity プロジェクトそのものを自動生成するものではありません**。
Unity の新規 2D プロジェクトを先に作り、その上に運用ファイルとディレクトリ構成を重ねる想定です。
