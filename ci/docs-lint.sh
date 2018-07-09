#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

markStartOfBlock "Running Docs Linter"
dotnet run -p tools/DocsLinter/DocsLinter.csproj 
markEndOfBlock "Running Docs Linter"