#!/usr/bin/env bash
set -euo pipefail

required_files=(
  "AGENTS.md"
  "README.md"
  ".gitignore"
  "docs/mvp-spec.md"
  "docs/issue-backlog.md"
  ".cursor/rules/unity.mdc"
)

for file in "${required_files[@]}"; do
  if [[ ! -f "$file" ]]; then
    echo "Missing required file: $file"
    exit 1
  fi
done

echo "Structure check passed."
