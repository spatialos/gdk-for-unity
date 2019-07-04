#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

ci/bootstrap.sh

source .shared-ci/scripts/pinned-tools.sh

echo "Running Docs Linter"
dotnet run -p .shared-ci/tools/DocsLinter/DocsLinter.csproj
