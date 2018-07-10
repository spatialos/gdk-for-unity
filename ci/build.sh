#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

markStartOfBlock "$0"

markStartOfBlock "Building Tools"

dotnet build -c Release tools/DocsLinter/DocsLinter.csproj
markEndOfBlock "Building Tools"

markStartOfBlock "Code Generation"

ci/codegen.sh

markEndOfBlock "Code Generation"

markEndOfBlock "$0"
