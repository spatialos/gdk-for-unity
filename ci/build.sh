#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

PROJECT_DIR="$(pwd)"

markStartOfBlock "$0"

markStartOfBlock "Building Tools"

dotnet build -c Release tools/DocsLinter/DocsLinter.csproj
markEndOfBlock "Building Tools"

markStartOfBlock "Code Generation"
spatial codegen
markEndOfBlock "Code Generation"

markStartOfBlock "Building Unity Project"
export UNITY_HOME="${UNITY_DIR}"
spatial worker build -t=local
markEndOfBlock "Building Unity Project"

markStartOfBlock "Code Generation End2End"
dotnet build -c Release code_generator/End2End/End2End.sln
markEndOfBlock "Code Generation End2End"

markEndOfBlock "$0"
