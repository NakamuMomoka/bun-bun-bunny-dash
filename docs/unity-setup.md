# Unity セットアップ手順

## 0. リポジトリルートと Unity プロジェクト

**リポジトリのルート = Unity プロジェクトのルート** として運用する。`ProjectSettings/` と `Packages/` はルートに置く。

同一リポジトリ内に別の Unity プロジェクト（ネストした `Library` を持つフォルダなど）を置かない。誤って作成した場合は Unity Editor を終了してからフォルダごと削除するか、ルートの `_archive/` へ退避する（ロックで消えないときはエディタと Hub を終了してから再試行）。`_archive/` は `.gitignore` 対象のため、退避物はローカル保管扱いになる。

## 初期セットアップのパターン

- **A（スターター上書き）**: Unity Hub で新規 2D Core プロジェクトを作り、このリポジトリの内容をそのルートへ上書きコピーする（下記「1」）。
- **B（clone して開く）**: リポジトリに `ProjectSettings/` が含まれる場合は、新規作成ではなく Unity Hub で **このリポジトリのルートフォルダを追加** して開く。

## 1. Unity プロジェクトを作る

- Unity Hub で新規 2D Core プロジェクトを作成する
- プロジェクト名は `Bun-Bun-Bunny-Dash` などの管理しやすい名前にする
- この ZIP の中身を、その Unity プロジェクトのルートへコピーする

## 2. GitHub リポジトリを作る

おすすめ repo 名:
- `bun-bun-bunny-dash`

作成後、Unity プロジェクトのルートで Git 初期化して push する。

## 3. Cursor を開く

- リポジトリルートを Cursor で開く
- `.cursor/rules/` と `AGENTS.md` を確認させる
- 最初は `docs/issue-backlog.md` の Issue 1 から始める

## 4. Unity MCP を使う場合

- Unity Editor 側の MCP が有効な状態で作業する
- Cursor から Scene / GameObject / Console を読めることを先に確認する
- いきなり大きな Scene 変更を任せず、1 Issue 1ゴールで進める

## 5. シーン作成の方針（固定名）

MVP では次の **3 シーン** を `Assets/Scenes/` に置く方針とする（名前と役割は変えない）。

| シーン名 | 役割 |
|---------|------|
| `TitleScene` | タイトル・開始前 |
| `GameScene` | メインゲームプレイ |
| `ResultScene` | ゲームオーバー・結果表示 |

Unity Editor 上で空シーンから作成し、ファイル名は上表と一致させる。詳細な画面要素は `docs/mvp-spec.md` の「シーン構成」を参照する。
