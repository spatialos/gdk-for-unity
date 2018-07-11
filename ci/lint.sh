#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

markStartOfBlock "$0"

# Check if the linter exists
set +e
command -v "${LINTER}" 2>&1
if [ $? -ne 0 ]; then
  echo "Please add ReSharper Command Line Tools to your path. See the readme for more details." >&2
  exit 1
fi
set -e

spatial codegen

# Ensure that all dependencies are present for Resharper.
dotnet restore code_generator/GdkCodeGenerator.sln
dotnet restore tools/Tools.sln

markStartOfBlock "Generating Solution Files"

PROJECT_DIR="$(pwd)"
touch "workers/unity/unity.sln"
pushd "workers/unity"
  dotnet run -p ../../tools/RunUnity/RunUnity.csproj -- \
    -projectPath "${PROJECT_DIR}/workers/unity" \
    -batchmode \
    -quit
popd

markEndOfBlock "Generating Solution Files"

markStartOfBlock "Linting tools"

${LINTER} --profile="IW Code Cleanup" \
  --settings=ReSharper2017.DotSettings \
  tools/Tools.sln

markEndOfBlock "Linting tools"

markStartOfBlock "Linting GDK Code Generator"

${LINTER} --profile="IW Code Cleanup" \
  --settings=ReSharper2017.DotSettings \
  --exclude=Generated/**/* \
  --exclude=Improbable.TextTemplating/**/* \
  --exclude=Mono.TextTemplating/**/* \
  code_generator/GdkCodeGenerator.sln

markEndOfBlock "Linting GDK Code Generator"

markStartOfBlock "Linting Unity GDK"

${LINTER} --profile="IW Code Cleanup" \
  --settings=ReSharper2017.DotSettings \
  --exclude=workers/unity/Assets/Improbable.Generated.NetworkTypes/**/* \
  workers/unity/unity.sln

markEndOfBlock "Linting Unity GDK"

markEndOfBlock "$0"
