SDK_VERSION="13.6.2"

spatial package get worker_sdk core-dynamic-x86_64-linux $SDK_VERSION ./Plugins/Improbable/Core/Linux/x86_64 --unzip -f
spatial package get worker_sdk core-bundle-x86_64-macos $SDK_VERSION ./Plugins/Improbable/Core/OSX --unzip -f
spatial package get worker_sdk core-dynamic-x86_64-win32 $SDK_VERSION ./Plugins/Improbable/Core/Windows/x86_64 --unzip -f

spatial package get worker_sdk csharp-c-interop $SDK_VERSION ./Plugins/Improbable/Sdk/Common --unzip -f

spatial package get schema standard_library $SDK_VERSION ./.schema --unzip -f

spatial package get tools schema_compiler-x86_64-win32 $SDK_VERSION ./.schema_compiler --unzip -f
spatial package get tools schema_compiler-x86_64-macos $SDK_VERSION ./.schema_compiler --unzip -f
spatial package get tools schema_compiler-x86_64-linux $SDK_VERSION ./.schema_compiler --unzip -f