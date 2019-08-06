#!/usr/bin/env bash
set -e -u -o pipefail

cd "$(dirname "$0")"

PKG_ROOT="workers/unity/Packages"
SDK_PATH="${PKG_ROOT}/io.improbable.worker.sdk"
SDK_MOBILE_PATH="${PKG_ROOT}/io.improbable.worker.sdk.mobile"
TEST_SDK_PATH="test-project/Packages/io.improbable.worker.sdk.testschema"
PLATFORM_SDK_PATH="${PKG_ROOT}/io.improbable.worker.sdk.platform"

SDK_VERSION="$(cat "${SDK_PATH}"/package.json | jq -r '.version')"

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

update_nuget_package() {
    local name=$1
    local version=$2
    local path=$3

    curl -sSL "https://www.nuget.org/api/v2/package/${name}/${version}" > ./tmp/${name}-${version}.zip

    unzip ./tmp/${name}-${version}.zip -d "${path}"

    local files=${4:-""}
    for file in $(echo $files | tr ";" "\n"); do
        rm -rf "${path}/${file}"
    done
}

mkdir -p ./tmp
update_nuget_package "Improbable.SpatialOS.Platform" "14.0.0" "${PLATFORM_SDK_PATH}/Plugins/Improbable" "_rels;lib/netstandard1.5;package;.signature.p7s;[Content_Types].xml;Improbable.SpatialOS.Platform.nuspec"
update_nuget_package "Google.Longrunning" "1.0.0" "${PLATFORM_SDK_PATH}/Plugins/Google/Longrunning" "_rels;lib/netstandard1.5;package;.signature.p7s;[Content_Types].xml;GoogleLongrunning.nuspec"
update_nuget_package "Google.Api.Gax.Grpc" "2.6.0" "${PLATFORM_SDK_PATH}/Plugins/Google/Api.Gax.Grpc" "_rels;lib/netstandard1.5;package;.signature.p7s;[Content_Types].xml;Google.Api.Gax.Grpc.nuspec"
update_nuget_package "Grpc.Core" "1.22.0" "${PLATFORM_SDK_PATH}/Plugins/Grpc/Core/" "_rels;lib/netstandard2.0;package;.signature.p7s;[Content_Types].xml;Grpc.Core.nuspec;native"
# update_nuget_package "Grpc.Core.Api" "1.22.0" "${PLATFORM_SDK_PATH}/Plugins/Grpc/Api/" "_rels;lib/netstandard1.5;package;.signature.p7s;[Content_Types].xml;GoogleLongrunning.nuspec"
rm -rf ./tmp

exit 0

# Update Core SDK
update_package worker_sdk core-dynamic-x86_64-linux "${SDK_PATH}/Plugins/Improbable/Core/Linux/x86_64"
update_package worker_sdk core-bundle-x86_64-macos "${SDK_PATH}/Plugins/Improbable/Core/OSX"
update_package worker_sdk core-dynamic-x86_64-win32 "${SDK_PATH}/Plugins/Improbable/Core/Windows/x86_64" "CoreSdkDll.lib"

update_package worker_sdk csharp-c-interop "${SDK_PATH}/Plugins/Improbable/Sdk/Common" "Improbable.Worker.CInterop.pdb"

update_package schema standard_library "${SDK_PATH}/.schema"
update_package schema test_schema_library "${TEST_SDK_PATH}/.schema" "test_schema/recursion.schema"

update_package tools schema_compiler-x86_64-win32 "${SDK_PATH}/.schema_compiler"
update_package tools schema_compiler-x86_64-macos "${SDK_PATH}/.schema_compiler"

#Update Mobile SDK
update_package worker_sdk core-static-fullylinked-arm-ios "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/iOS/arm" "CoreSdkStatic.lib;libCoreSdkStatic.a.pic"
update_package worker_sdk core-static-fullylinked-x86_64-ios "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/iOS/x86_64" "CoreSdkStatic.lib;libCoreSdkStatic.a.pic"

update_package worker_sdk core-dynamic-arm64v8a-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/arm64"
update_package worker_sdk core-dynamic-armv7a-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/armv7"
update_package worker_sdk core-dynamic-x86-android "${SDK_MOBILE_PATH}/Plugins/Improbable/Core/Android/x86"

update_package worker_sdk csharp-c-interop-static "${SDK_MOBILE_PATH}/Plugins/Improbable/Sdk/iOS" "Improbable.Worker.CInteropStatic.pdb"
