#!/usr/bin/env bash

# Print the .NETCore version to aid debugging,
# as well as ensuring that later calls to the tool don't print the welcome message on first run.
dotnet --version

export LINTER="cleanupcode.exe"

DOTNET_VERSION="$(dotnet --version)"

export MSBuildSDKsPath="${PROGRAMFILES}/dotnet/sdk/${DOTNET_VERSION}/Sdks"

pushd workers/unity
    UNITY_DIR="$(dotnet run -p ../../tools/FindUnity/FindUnity.csproj)"
popd
