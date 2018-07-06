#!/usr/bin/env bash

error() {
   local SOURCE_FILE=$1
   local LINE_NO=$2
   echo "ERROR: ${SOURCE_FILE}(${LINE_NO}):"
}
trap 'error "${BASH_SOURCE}" "${LINENO}"' ERR

UNITY_VERSION=$(cat "workers/unity/ProjectSettings/ProjectVersion.txt" | grep "m_EditorVersion" | cut -d ' ' -f2)

function isUnityHomeSet() {
  UNITY_HOME=${UNITY_HOME:-}
  [ -n "$UNITY_HOME" ]
}

function isUnityImprobablePathPresent() {
  UNITY_DIR="C:/Unity/Unity-${UNITY_VERSION}/"
  [ -d "$UNITY_DIR" ]
}

function getUnityDir() {
  if isUnityHomeSet; then
    echo "${UNITY_HOME}"
  elif isUnityImprobablePathPresent; then
    echo "${UNITY_DIR}"
  else
    echo "ERROR: Unity was not found in the default location. Please set the UNITY_HOME environment variable to where Unity ${UNITY_VERSION} is installed." >&2
    exit 1
  fi
}

UNITY_DIR="$(getUnityDir)"
export UNITY_EXE="${UNITY_DIR}/Editor/Unity.exe"

export LINTER="cleanupcode.exe"

export MSBUILD="$(powershell  –ExecutionPolicy Bypass ./ci/find-msbuild.ps1)"

export NUNIT3_CONSOLE="code_generator/packages/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe"

function unpackTo() {
  local SOURCE=$1
  local TARGET=$2

  mkdir -p "${TARGET}"
  unzip -o -q "${SOURCE}" -d "${TARGET}"
}

PINNED_CORE_SDK_VERSION="13.0.1"
