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

markStartOfBlock "Code Generation End2End"
dotnet build -c Release code_generator/End2End/End2End.sln
markEndOfBlock "Code Generation End2End"

markEndOfBlock "$0"
