#!/usr/bin/env bash

set -e -u -x -o pipefail

cd "$(dirname "$0")"

source ci/includes/pinned-tools.sh

WORKER_BUILD_SNAPSHOT="14.0-b5142-2899b-WORKER-SNAPSHOT"

CORE_API_DIR="$(pwd)/workers/unity/Assets/Plugins/Improbable/Core"

TEMP_DIR="$(pwd)/tmp/worker_sdk"


rm -rf ${TEMP_DIR}
mkdir -p ${TEMP_DIR}

spatial package retrieve "worker_sdk" "c-dynamic-x86_64-msvc_mt-win32" "${WORKER_BUILD_SNAPSHOT}" "${TEMP_DIR}/core-api-win"
spatial package retrieve "worker_sdk" "c-dynamic-x86_64-clang_libcpp-macos" "${WORKER_BUILD_SNAPSHOT}" "${TEMP_DIR}/core-api-macos"
spatial package retrieve "worker_sdk" "c-dynamic-x86_64-gcc_libstdcpp-linux" "${WORKER_BUILD_SNAPSHOT}" "${TEMP_DIR}/core-api-linux"


rm -rf ${CORE_API_DIR}
mkdir -p ${CORE_API_DIR}

unpackTo "${TEMP_DIR}/core-api-win" "${CORE_API_DIR}/Windows/x86_64/"
unpackTo "${TEMP_DIR}/core-api-macos" "${CORE_API_DIR}/OSX/"
unpackTo "${TEMP_DIR}/core-api-linux" "${CORE_API_DIR}/Linux/x86_64/"

rm -rf ${TEMP_DIR}