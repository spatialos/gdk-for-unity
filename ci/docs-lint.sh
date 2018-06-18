#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

markStartOfBlock "Building Docs Linter"

"${MSBUILD}" tools/ImpNuget/ImpNuget.csproj //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2
pushd "tools/DocsLinter"
    "../../bin/ImpNuget/ImpNuget.exe"
popd

"${MSBUILD}" tools/DocsLinter/DocsLinter.csproj //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2

markEndOfBlock "Building Docs Linter"

markStartOfBlock "Running Docs Linter"
bin/DocsLinter/DocsLinter.exe  
markEndOfBlock "Running Docs Linter"