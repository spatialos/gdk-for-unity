#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

ci/bootstrap.sh

source .shared-ci/scripts/pinned-tools.sh
source .shared-ci/scripts/profiling.sh

markStartOfBlock "Running Docs Linter"
dotnet run -p .shared-ci/tools/DocsLinter/DocsLinter.csproj 
markEndOfBlock "Running Docs Linter"