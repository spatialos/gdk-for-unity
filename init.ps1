param (
    [switch]$china=$false
)

Set-Location $PSScriptRoot

$EnvironmentArgs = ""
if($china) {
    Write-Host "Downloading with cn-production environment"
    $EnvironmentArgs="--environment=cn-production"
}

$PkgRoot = $PSScriptRoot + "/workers/unity/Packages"
$SdkPath = $PkgRoot + "/io.improbable.worker.sdk"
$SdkMobilePath = $PkgRoot + "/io.improbable.worker.sdk.mobile"
$TestSdkPath="test-project/Packages/io.improbable.worker.sdk.testschema"

if (Test-Path env:WORKER_SDK_OVERRIDE) {
    $SdkVersion = $env:WORKER_SDK_OVERRIDE;
} else {
    $SdkVersion = Get-Content ($SdkPath + "/.sdk.version")
}

$SpotVersion = Get-Content ($SdkPath + "/.spot.version")

function UpdatePackage($type, $identifier, $path, $removes)
{
    spatial package get $type $identifier $SdkVersion "$path" --unzip --force --json_output $EnvironmentArgs

    if ($null -ne $removes)
    {
        $removes.Split(";") | ForEach-Object {
            Remove-Item "$path/$_"
        }
    }
}

function UpdateSpot($identifier, $path)
{
    spatial package get spot $identifier $SpotVersion "$path" --force --json_output $EnvironmentArgs
}

UpdatePackage worker_sdk c-dynamic-x86_64-gcc510-linux "$SdkPath/Plugins/Improbable/Core/Linux/x86_64"
UpdatePackage worker_sdk c-bundle-x86_64-clang-macos "$SdkPath/Plugins/Improbable/Core/OSX"
UpdatePackage worker_sdk c-dynamic-x86_64-vc141_mt-win32 "$SdkPath/Plugins/Improbable/Core/Windows/x86_64" "improbable_worker.lib"

UpdatePackage worker_sdk csharp_cinterop "$SdkPath/Plugins/Improbable/Sdk/Common"

UpdatePackage schema standard_library "$SdkPath/.schema"
UpdatePackage schema test_schema_library "schema"

UpdatePackage tools schema_compiler-x86_64-win32 "$SdkPath/.schema_compiler"
UpdatePackage tools schema_compiler-x86_64-macos "$SdkPath/.schema_compiler"

UpdateSpot spot-win64 "$SdkPath/.spot/spot.exe"
UpdateSpot spot-macos "$SdkPath/.spot/spot"

#Update Mobile SDK
UpdatePackage worker_sdk c-static-arm-clang-ios "$SdkMobilePath/Plugins/Improbable/Core/iOS/arm"
UpdatePackage worker_sdk c-static-x86_64-clang-ios "$SdkMobilePath/Plugins/Improbable/Core/iOS/x86_64"

UpdatePackage worker_sdk c-dynamic-arm64v8a-clang_ndk21-android "$SdkMobilePath/Plugins/Improbable/Core/Android/arm64"
UpdatePackage worker_sdk c-dynamic-armv7a-clang_ndk21-android "$SdkMobilePath/Plugins/Improbable/Core/Android/armv7"

UpdatePackage worker_sdk csharp_cinterop_static "$SdkMobilePath/Plugins/Improbable/Sdk/iOS"
