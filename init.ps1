cd $PSScriptRoot

$PkgRoot = $PSScriptRoot + "/workers/unity/Packages"
$SdkPath = $PkgRoot + "/io.improbable.worker.sdk"
$SdkMobilePath = $PkgRoot + "/io.improbable.worker.sdk.mobile"

$SdkVersion = Get-Content ($SdkPath + "/package.json") | jq -r '.version'

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

UpdatePackage worker_sdk core-dynamic-x86_64-linux "$SdkPath/Plugins/Improbable/Core/Linux/x86_64"
UpdatePackage worker_sdk core-bundle-x86_64-macos "$SdkPath/Plugins/Improbable/Core/OSX"
UpdatePackage worker_sdk core-dynamic-x86_64-win32 "$SdkPath/Plugins/Improbable/Core/Windows/x86_64" "CoreSdkDll.lib"

UpdatePackage worker_sdk csharp-c-interop "$SdkPath/Plugins/Improbable/Sdk/Common" "Improbable.Worker.CInterop.pdb"

UpdatePackage schema standard_library "$SdkPath/.schema"

UpdatePackage tools schema_compiler-x86_64-win32 "$SdkPath/.schema_compiler"
UpdatePackage tools schema_compiler-x86_64-macos "$SdkPath/.schema_compiler"
UpdatePackage tools schema_compiler-x86_64-linux "$SdkPath/.schema_compiler"

#Update Mobile SDK
UpdatePackage worker_sdk core-static-fullylinked-arm-ios "$SdkMobilePath/Plugins/Improbable/Core/iOS/arm" "CoreSdkStatic.lib;libCoreSdkStatic.a.pic"
UpdatePackage worker_sdk core-static-fullylinked-x86_64-ios "$SdkMobilePath/Plugins/Improbable/Core/iOS/x86_64" "CoreSdkStatic.lib;libCoreSdkStatic.a.pic"

UpdatePackage worker_sdk core-dynamic-arm64v8a-android "$SdkMobilePath/Plugins/Improbable/Core/Android/arm64"
UpdatePackage worker_sdk core-dynamic-armv7a-android "$SdkMobilePath/Plugins/Improbable/Core/Android/armv7"
UpdatePackage worker_sdk core-dynamic-x86-android "$SdkMobilePath/Plugins/Improbable/Core/Android/x86"

UpdatePackage worker_sdk csharp-c-interop-static "$SdkMobilePath/Plugins/Improbable/Sdk/iOS" "Improbable.Worker.CInteropStatic.pdb"