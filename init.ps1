cd $PSScriptRoot

$PkgRoot = $PSScriptRoot + "/workers/unity/Packages"
$SdkPath = $PkgRoot + "/io.improbable.worker.sdk"
$SdkMobilePath = $PkgRoot + "/io.improbable.worker.sdk.mobile"
$TestSdkPath="test-project/Packages/io.improbable.worker.sdk.testschema"

$SdkVersion = "14.0.2"
# $SdkVersion = Get-Content ($SdkPath + "/package.json") | jq -r '.version'
$SpotVersion = Get-Content ($SdkPath + "/.spot.version")

function UpdatePackage($type, $identifier, $path, $removes)
{
    spatial package get $type $identifier $SdkVersion "$path" --unzip --force --json_output

    if ($removes -ne $null)
    {
        $removes.Split(";") | ForEach {
            rm "$path/$_"
        }
    }
}

function UpdateSpot($identifier, $path)
{
    spatial package get spot $identifier $SpotVersion "$path" --force --json_output
}

UpdatePackage worker_sdk c-dynamic-x86_64-gcc510-linux "$SdkPath/Plugins/Improbable/Core/Linux/x86_64"
UpdatePackage worker_sdk c-bundle-x86_64-clang-macos "$SdkPath/Plugins/Improbable/Core/OSX"
UpdatePackage worker_sdk c-dynamic-x86_64-vc140_mt-win32 "$SdkPath/Plugins/Improbable/Core/Windows/x86_64" "improbable_worker.lib"

UpdatePackage worker_sdk csharp_cinterop "$SdkPath/Plugins/Improbable/Sdk/Common" "Improbable.Worker.CInterop.pdb"

UpdatePackage schema standard_library "$SdkPath/.schema"
UpdatePackage schema test_schema_library "$TestSdkPath/.schema"

UpdatePackage tools schema_compiler-x86_64-win32 "$SdkPath/.schema_compiler"
UpdatePackage tools schema_compiler-x86_64-macos "$SdkPath/.schema_compiler"

UpdateSpot spot-win64 "$SdkPath/.spot/spot.exe"
UpdateSpot spot-macos "$SdkPath/.spot/spot"

#Update Mobile SDK
UpdatePackage worker_sdk c-static-fullylinked-arm-clang-ios "$SdkMobilePath/Plugins/Improbable/Core/iOS/arm" "improbable_worker_static.lib;libimprobable_worker_static.a.pic"
UpdatePackage worker_sdk c-static-fullylinked-x86_64-clang-ios "$SdkMobilePath/Plugins/Improbable/Core/iOS/x86_64" "improbable_worker_static.lib;libimprobable_worker_static.a.pic"

UpdatePackage worker_sdk c-dynamic-arm64v8a-clang_ndk16b-android "$SdkMobilePath/Plugins/Improbable/Core/Android/arm64"
UpdatePackage worker_sdk c-dynamic-armv7a-clang_ndk16b-android "$SdkMobilePath/Plugins/Improbable/Core/Android/armv7"
UpdatePackage worker_sdk c-dynamic-x86-clang_ndk16b-android "$SdkMobilePath/Plugins/Improbable/Core/Android/x86"

UpdatePackage worker_sdk csharp_cinterop_static "$SdkMobilePath/Plugins/Improbable/Sdk/iOS" "Improbable.Worker.CInteropStatic.pdb"
