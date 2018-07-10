#!/usr/bin/env bash

IMPROBABLE_UNITY_ROOT="C:/Unity"
UNITY_VERSION=$(cat "workers/unity/ProjectSettings/ProjectVersion.txt" | grep "m_EditorVersion" | cut -d ' ' -f2)

function isUnityHomeSet() {
  UNITY_HOME=${UNITY_HOME:-}
  [ -n "$UNITY_HOME" ]
}

function isUnityImprobablePathPresent() {
  [ -d "$UNITY_DIR" ]
}

function getUnityDir() {
  if isUnityHomeSet; then
    echo "${UNITY_HOME}"
  else
    echo "${IMPROBABLE_UNITY_ROOT}/${UNITY_VERSION}/"
  fi
}

UNITY_DIR="$(getUnityDir)"
export UNITY_HOME="${UNITY_DIR}"
export UNITY_EXE="${UNITY_DIR}/Editor/Unity.exe"

export LINTER="cleanupcode.exe"

DOTNET_VERSION="$(dotnet --version)"

export MSBuildSDKsPath="${PROGRAMFILES}/dotnet/sdk/${DOTNET_VERSION}/Sdks"
