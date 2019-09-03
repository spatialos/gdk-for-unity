#!/usr/bin/env bash
set -e -u -o pipefail

cd "$(dirname "$0")"

PKG_ROOT="workers/unity/Packages"
SDK_PATH="${PKG_ROOT}/io.improbable.worker.sdk"
SDK_MOBILE_PATH="${PKG_ROOT}/io.improbable.worker.sdk.mobile"
TEST_SDK_PATH="test-project/Packages/io.improbable.worker.sdk.testschema"

SDK_VERSION="14.0.2"
# SDK_VERSION="$(cat "${SDK_PATH}"/package.json | jq -r '.version')"
SPOT_VERSION="$(cat "${SDK_PATH}"/.spot.version)"

update_package() {
    local type=$1
    local identifier=$2
    local path=$3

    spatial package get $type $identifier $SDK_VERSION "${path}" --unzip --force --json_output

    local files=${4:-""}
    for file in $(echo $files | tr ";" "\n"); do
        rm "${path}/${file}"
    done
}

update_spot() {
    local identifer=$1
    local path=$2

    spatial package get spot $identifer $SPOT_VERSION "${path}" --force --json_output
}

# Update Core SDK
update_package worker_sdk c-dynamic-x86_64-gcc510-linux "${SDK_PATH}/Plugins/Improbable/Core/Linux/x86_64"
update_package worker_sdk c-bundle-x86_64-clang-macos "${SDK_PATH}/Plugins/Improbable/Core/OSX"
update_package worker_sdk c-dynamic-x86_64-vc140_mt-win32 "${SDK_PATH}/Plugins/Improbable/Core/Windows/x86_64" "improbable_worker.lib"

update_package worker_sdk csharp_cinterop "${SDK_PATH}/Plugins/Improbable/Sdk/Common" "Improbable.Worker.CInterop.pdb"

update_package schema standard_library "${SDK_PATH}/.schema"
update_package schema test_schema_library "${TEST_SDK_PATH}/.schema"

update_package tools schema_compiler-x86_64-win32 "${SDK_PATH}/.schema_compiler"
update_package tools schema_compiler-x86_64-macos "${SDK_PATH}/.schema_compiler"

update_spot spot-win64 "${SDK_PATH}/.spot/spot.exe"
update_spot spot-macos "${SDK_PATH}/.spot/spot"

#Update Mobile SDK
update_package worker_sdk c-static-fullylinked-arm-clang-ios "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/iOS/arm" "improbable_worker_static.lib;libimprobable_worker_static.a.pic"
update_package worker_sdk c-static-fullylinked-x86_64-clang-ios "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/iOS/x86_64" "improbable_worker_static.lib;libimprobable_worker_static.a.pic"

update_package worker_sdk c-dynamic-arm64v8a-clang_ndk16b-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/arm64"
update_package worker_sdk c-dynamic-armv7a-clang_ndk16b-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/armv7"
update_package worker_sdk c-dynamic-x86-clang_ndk16b-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/x86"

update_package worker_sdk csharp_cinterop_static "${SDK_MOBILE_PATH}/Plugins/Improbable/Sdk/iOS" "Improbable.Worker.CInteropStatic.pdb"
