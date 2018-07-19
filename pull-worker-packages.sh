#!/usr/bin/env bash

set -e -u -x -o pipefail

cd "$(dirname "$0")"

source ci/includes/pinned-tools.sh

WORKER_BUILD_SNAPSHOT="14.0-b5282-2f740-WORKER-SNAPSHOT"

CORE_API_DIR="$(pwd)/workers/unity/Assets/Plugins/Improbable/Core"
SDK_DIR="$(pwd)/workers/unity/Assets/Plugins/Improbable/Sdk"

TEMP_DIR="$(pwd)/tmp/worker_sdk"


rm -rf ${TEMP_DIR}
mkdir -p ${TEMP_DIR}

spatial package retrieve "worker_sdk" "c-dynamic-x86_64-msvc_mt-win32" "${WORKER_BUILD_SNAPSHOT}" "${TEMP_DIR}/core-api-win"
spatial package retrieve "worker_sdk" "csharp_core" "${WORKER_BUILD_SNAPSHOT}" "${TEMP_DIR}/csharp_core"

rm -rf ${CORE_API_DIR}
mkdir -p ${CORE_API_DIR}

unpackTo "${TEMP_DIR}/core-api-win" "${CORE_API_DIR}/Windows/x86_64/"

rm -rf ${SDK_DIR}
mkdir -p ${SDK_DIR}

unpackTo "${TEMP_DIR}/csharp_core" "${SDK_DIR}"

rm -rf ${TEMP_DIR}