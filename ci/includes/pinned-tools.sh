#!/usr/bin/env bash

# Print the .NETCore version to aid debugging,
# as well as ensuring that later calls to the tool don't print the welcome message on first run.
dotnet --version

export LINTER="cleanupcode.exe"

MSBUILD="$(powershell –ExecutionPolicy Bypass ./ci/find-msbuild.ps1)"
export MSBUILD

export NUNIT3_CONSOLE="code_generator/packages/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe"

pushd workers/unity
    UNITY_DIR="$(dotnet run -p ../../tools/FindUnity/FindUnity.csproj)"
popd

export UNITY_HOME="${UNITY_DIR}"
export UNITY_EXE="${UNITY_DIR}/Editor/Unity.exe"
