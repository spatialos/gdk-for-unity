#!/usr/bin/env bash
set -e -u -o pipefail

cd "$(dirname "$0")"

PKG_ROOT="../../workers/unity/Packages"
SDK_PATH="$PKG_ROOT/io.improbable.worker.sdk"
SDK_MOBILE_PATH="$PKG_ROOT/io.improbable.worker.sdk.mobile"

SDK_VERSION="$(cat "${SDK_PATH}"/package.json | jq -r '.version')"

update_package() {
    local type=$1
    local identifier=$2
    local path=$3

    spatial package get $type $identifier $SDK_VERSION "${path}" --unzip --force --json_output

    local files=${4:-""}
    IFS=';' read -a files <<< "$files"
    for file in "${files[@]}"; do
        rm "${path}/${file}"
    done
}

# Update Core SDK
update_package worker_sdk core-dynamic-x86_64-linux "${SDK_PATH}/Plugins/Improbable/Core/Linux/x86_64"
update_package worker_sdk core-bundle-x86_64-macos "${SDK_PATH}/Plugins/Improbable/Core/OSX"
update_package worker_sdk core-dynamic-x86_64-win32 "${SDK_PATH}/Plugins/Improbable/Core/Windows/x86_64" "CoreSdkDll.lib"

update_package worker_sdk csharp-c-interop "${SDK_PATH}/Plugins/Improbable/Sdk/Common" "Improbable.Worker.CInterop.pdb"

update_package schema standard_library "${SDK_PATH}/.schema"

update_package tools schema_compiler-x86_64-win32 "${SDK_PATH}/.schema_compiler"
update_package tools schema_compiler-x86_64-macos "${SDK_PATH}/.schema_compiler"
update_package tools schema_compiler-x86_64-linux "${SDK_PATH}/.schema_compiler"

#Update Mobile SDK
update_package worker_sdk core-static-fullylinked-arm-ios "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/iOS/arm" "CoreSdkStatic.lib;libCoreSdkStatic.a.pic"
update_package worker_sdk core-static-fullylinked-x86_64-ios "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/iOS/x86_64" "CoreSdkStatic.lib;libCoreSdkStatic.a.pic"

update_package worker_sdk core-dynamic-arm64v8a-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/arm64"
update_package worker_sdk core-dynamic-armv7a-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/armv7"
update_package worker_sdk core-dynamic-x86-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/x86"

update_package worker_sdk csharp-c-interop-static "${SDK_MOBILE_PATH}/Plugins/Improbable/Sdk/iOS" "Improbable.Worker.CInteropStatic.pdb"
