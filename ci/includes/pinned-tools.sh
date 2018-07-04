#!/usr/bin/env bash

UNITY_ROOT="C:/Unity"
UNITY_VERSION=$(cat "workers/unity/ProjectSettings/ProjectVersion.txt" | grep "m_EditorVersion" | cut -d ' ' -f2)

function isLinux() {
  [[ "$(uname -s)" == "Linux" ]];
}

function isMacOS() {
  [[ "$(uname -s)" == "Darwin" ]];
}

function isWindows() {
  ! ( isLinux || isMacOS );
}

function isUnityHomeSet() {
  UNITY_HOME=${UNITY_HOME:-}
  [ -n "$UNITY_HOME" ]
}

function isTeamCity() {
  # -n == string comparison "not null"
  [ -n "${TEAMCITY_CAPTURE_ENV+x}" ]
}

function isNotTeamCity() {
  # -z == string comparison "null, that is, 0-length"
  [ -z "${TEAMCITY_CAPTURE_ENV+x}" ]
}

function isUnityImprobablePathPresent() {
  UNITY_DIR="${UNITY_ROOT}/${UNITY_VERSION}/"
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
export UNITY_EXE=$(printf %q "${UNITY_DIR}/Editor/Unity.exe")

export LINTER="cleanupcode.exe"

export MSBUILD="$(powershell  –ExecutionPolicy Bypass ./ci/find-msbuild.ps1)"

export NUNIT3_CONSOLE="code_generator/packages/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe"
