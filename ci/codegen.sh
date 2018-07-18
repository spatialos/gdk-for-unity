#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

markStartOfBlock "Copying Packages Schema"

dotnet run -p tools/CopySchema/CopySchema.csproj -- \
  "workers/unity/Packages/manifest.json" \
  "schema"

markEndOfBlock "Copying Packages Schema"

markStartOfBlock "Generating code"

if isWindows;
  then SCHEMA_COMPILER="tools/schema_compiler/win/schema_compiler.exe"
elif isMacOS;
  then SCHEMA_COMPILER="tools/schema_compiler/macos/schema_compiler"
fi

dotnet run -p code_generator/GdkCodeGenerator/GdkCodeGenerator.csproj -- \
  --schema-compiler-path="$SCHEMA_COMPILER" \
  --schema-path="schema" \
  --schema-path="build/dependencies/schema/standard_library" \
  --json-dir="workers/unity/Temp/ImprobableJson" \
  --native-output-dir="workers/unity/Assets/Generated/Source" \
  --network-types-output-dir="workers/unity/Assets/Improbable.Generated.NetworkTypes/Generated"

markEndOfBlock "Generating code"
