#!/usr/bin/env bash

export LINTER="cleanupcode.exe"

MSBUILD="$(powershell –ExecutionPolicy Bypass ./ci/find-msbuild.ps1)"
export MSBUILD

export NUNIT3_CONSOLE="code_generator/packages/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe"

"${MSBUILD}" tools/FindUnity/FindUnity.csproj //property:Configuration=Release //clp:ErrorsOnly //nologo //m 1>&2

pushd workers/unity
    UNITY_DIR="$(../../tools/FindUnity/bin/Release/FindUnity.exe)"
popd

export UNITY_HOME="${UNITY_DIR}"
export UNITY_EXE="${UNITY_DIR}/Editor/Unity.exe"