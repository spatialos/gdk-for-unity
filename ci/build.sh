#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

PROJECT_DIR="$(pwd)"

markStartOfBlock "$0"

markStartOfBlock "Building Tools"
"${MSBUILD}" tools/ImpNuget/ImpNuget.csproj //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2
pushd "tools/DocsLinter"
    "../../bin/ImpNuget/ImpNuget.exe"
popd

"${MSBUILD}" tools/DocsLinter/DocsLinter.csproj //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2
markEndOfBlock "Building Tools"

markStartOfBlock "Code Generation"
spatial codegen
markEndOfBlock "Code Generation"

markStartOfBlock "Building Unity Project"
export UNITY_HOME="${UNITY_DIR}"
spatial worker build -t=local
markEndOfBlock "Building Unity Project"

markStartOfBlock "Code Generation End2End"
pushd "code_generator/End2End/Tests"
    "../../../bin/ImpNuget/ImpNuget.exe"
popd

"${MSBUILD}" code_generator/End2End/End2End.sln //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2
markEndOfBlock "Code Generation End2End"

markEndOfBlock "$0"
