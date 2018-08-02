#!/usr/bin/env bash

# Modify "../core-sdk.version" and run this script to download the new version of the CoreSDK and supporting tools.

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

function cleanAndUnpackTo() {
  local SOURCE=$1
  local TARGET=$2

  rm -rf "${TARGET}"
  mkdir -p "${TARGET}"
  unzip -o -q "${SOURCE}" -d "${TARGET}"
}

PINNED_CORE_SDK_VERSION="$(cat core-sdk.version)"

CORE_SDK_DIR="$(pwd)/build/core_sdk"
UNITY_PROJECT_DIR="$(pwd)/workers/unity"
NATIVE_DEPENDENCIES_PATH="${UNITY_PROJECT_DIR}/Assets/Plugins/Improbable/Core"
MANAGED_DEPENDENCIES_PATH="${UNITY_PROJECT_DIR}/Assets/Plugins/Improbable/Sdk"

rm -rf "${CORE_SDK_DIR}"

mkdir -p "${CORE_SDK_DIR}/schema"
mkdir -p "${CORE_SDK_DIR}/tools"
mkdir -p "${CORE_SDK_DIR}/worker_sdk"

spatial package retrieve "worker_sdk" "core-dynamic-x86-win32"       "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86-win32"
spatial package retrieve "worker_sdk" "core-dynamic-x86_64-win32"    "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86_64-win32"
spatial package retrieve "worker_sdk" "core-dynamic-x86_64-linux"    "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86_64-linux"
spatial package retrieve "worker_sdk" "core-bundle-x86_64-macos"     "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/worker_sdk/core-bundle-x86_64-macos"
spatial package retrieve "worker_sdk" "csharp"                       "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/worker_sdk/csharp"
spatial package retrieve "tools"      "schema_compiler-x86_64-win32" "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/tools/schema_compiler-x86_64-win32"
spatial package retrieve "tools"      "schema_compiler-x86_64-macos" "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/tools/schema_compiler-x86_64-macos"
spatial package retrieve "schema"     "standard_library"             "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/schema/standard_library"

cleanAndUnpackTo "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86-win32"         "${NATIVE_DEPENDENCIES_PATH}/Windows/x86"
cleanAndUnpackTo "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86_64-win32"      "${NATIVE_DEPENDENCIES_PATH}/Windows/x86_64"
cleanAndUnpackTo "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86_64-linux"      "${NATIVE_DEPENDENCIES_PATH}/Linux/x86_64"
cleanAndUnpackTo "${CORE_SDK_DIR}/worker_sdk/core-bundle-x86_64-macos"       "${NATIVE_DEPENDENCIES_PATH}/OSX"
cleanAndUnpackTo "${CORE_SDK_DIR}/worker_sdk/csharp"                         "${MANAGED_DEPENDENCIES_PATH}"
cleanAndUnpackTo "${CORE_SDK_DIR}/tools/schema_compiler-x86_64-win32"        "tools/schema_compiler/win"
cleanAndUnpackTo "${CORE_SDK_DIR}/tools/schema_compiler-x86_64-macos"        "tools/schema_compiler/macos"

# `spatial local launch` and `spatial upload` require these to be here, until the new project structure is applied.
cleanAndUnpackTo "${CORE_SDK_DIR}/build/schema/standard_library"             "build/dependencies/schema/standard_library"

# Remove unused tools and files.
rm tools/schema_compiler/win/protoc.exe
rm tools/schema_compiler/macos/protoc
rm -rf tools/schema_compiler/win/proto
rm -rf tools/schema_compiler/macos/proto
