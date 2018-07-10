#!/usr/bin/env bash
error() {
   local SOURCE_FILE=$1
   local LINE_NO=$2
   echo "ERROR: ${SOURCE_FILE}(${LINE_NO}):"
}

function unpackTo() {
  local SOURCE=$1
  local TARGET=$2

  mkdir -p "${TARGET}"
  unzip -o -q "${SOURCE}" -d "${TARGET}"
}

# Print the .NETCore version to aid debugging,
# as well as ensuring that later calls to the tool don't print the welcome message on first run.
dotnet --version

export LINTER="cleanupcode.exe"

DOTNET_VERSION="$(dotnet --version)"

export MSBuildSDKsPath="${PROGRAMFILES}/dotnet/sdk/${DOTNET_VERSION}/Sdks"

PINNED_CORE_SDK_VERSION="$(cat core-sdk.version)"

pushd workers/unity
    UNITY_DIR="$(dotnet run -p ../../tools/FindUnity/FindUnity.csproj)"
popd
