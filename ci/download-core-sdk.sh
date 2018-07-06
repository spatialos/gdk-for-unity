#!/usr/bin/env bash

# Modify the PINNED_CORE_SDK_VERSION variable in `ci/includes/pinned-tools.sh` and run this script to download
# the new version of the CoreSDK and supporting tools.

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

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
spatial package retrieve "schema"     "standard_library"             "${PINNED_CORE_SDK_VERSION}" "${CORE_SDK_DIR}/schema/standard_library"

unpackTo "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86-win32"         "${NATIVE_DEPENDENCIES_PATH}/Windows/x86"
unpackTo "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86_64-win32"      "${NATIVE_DEPENDENCIES_PATH}/Windows/x86_64"
unpackTo "${CORE_SDK_DIR}/worker_sdk/core-dynamic-x86_64-linux"      "${NATIVE_DEPENDENCIES_PATH}/Linux/x86_64"
unpackTo "${CORE_SDK_DIR}/worker_sdk/core-bundle-x86_64-macos"       "${NATIVE_DEPENDENCIES_PATH}/OSX"
unpackTo "${CORE_SDK_DIR}/worker_sdk/csharp"                         "${MANAGED_DEPENDENCIES_PATH}"
unpackTo "${CORE_SDK_DIR}/tools/schema_compiler-x86_64-win32"        "tools/schema_compiler/win"
unpackTo "${CORE_SDK_DIR}/schema/standard_library"                   "schema_standard_library"
