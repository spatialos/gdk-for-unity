SDK_VERSION="13.6.2"

spatial package get worker_sdk core-static-fullylinked-arm-ios $SDK_VERSION ./Plugins/Improbable/Core/iOS/arm --unzip -f
spatial package get worker_sdk core-static-fullylinked-x86_64-ios $SDK_VERSION ./Plugins/Improbable/iOS/x86_64 --unzip -f

spatial package get worker_sdk core-dynamic-arm64-android $SDK_VERSION ./Plugins/Improbable/Core/Android/arm64 --unzip -f
spatial package get worker_sdk core-dynamic-armeabi_v7a-android $SDK_VERSION ./Plugins/Improbable/Core/Android/armv7 --unzip -f
spatial package get worker_sdk core-dynamic-x86-android-android $SDK_VERSION ./Plugins/Improbable/Core/Android/x86 --unzip -f

spatial package get worker_sdk csharp-c-interop-static $SDK_VERSION ./Plugins/Improbable/Sdk/iOS --unzip -f