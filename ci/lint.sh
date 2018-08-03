#!/usr/bin/env bash
set -e -u -o pipefail
if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

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

CODE_GENERATOR_DIR="workers/unity/Packages/com.improbable.gdk.tools/.CodeGenerator"

# Ensure that all dependencies are present for Resharper.
dotnet restore "${CODE_GENERATOR_DIR}/GdkCodeGenerator.sln"
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
  --disable-settings-layers="SolutionPersonal;GlobalAll;GlobalPerProduct" \
  tools/Tools.sln

markEndOfBlock "Linting tools"

markStartOfBlock "Linting GDK Code Generator"

${LINTER} --profile="IW Code Cleanup" \
  --settings="${CODE_GENERATOR_DIR}/GdkCodeGenerator.sln.DotSettings" \
  --disable-settings-layers="SolutionPersonal;GlobalAll;GlobalPerProduct" \
  "${CODE_GENERATOR_DIR}/GdkCodeGenerator.sln"

markEndOfBlock "Linting GDK Code Generator"

markStartOfBlock "Linting Unity GDK"

# We've setup ReSharper to ignore the Generated code projects, which means it will spuriously delete all `using`
# references to them unless we copy them from the location that Unity has built them into where the .csproj's
# <OutputPath> says they'll be.
mkdir -p workers/unity/Temp/bin/Debug
cp -r workers/unity/Library/ScriptAssemblies/*Generated*.dll workers/unity/Temp/bin/Debug/

${LINTER} --profile="IW Code Cleanup" \
  --settings=workers/unity/unity.sln.DotSettings \
  --disable-settings-layers="SolutionPersonal;GlobalAll;GlobalPerProduct" \
  workers/unity/unity.sln

markEndOfBlock "Linting Unity GDK"

markEndOfBlock "$0"
