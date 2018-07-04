#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

function isLinux() {
  [[ "$(uname -s)" == "Linux" ]];
}

function isMacOS() {
  [[ "$(uname -s)" == "Darwin" ]];
}

function isWindows() {
  ! ( isLinux || isMacOS );
}

# Only run inside TeamCity
if [ -z "${TEAMCITY_CAPTURE_ENV+x}" ]; then
    exit 0
fi

if isWindows; then
    MODULES="linux mac-mono"
elif isMacOS; then
    MODULES="linux windows"
elif isLinux; then
    echo "Building is not supported on linux"
    exit 1
fi

GOPATH="$(pwd)/go"
UNITY_PACKAGE="github.com/improbable/unity_downloader"

markStartOfBlock "$0"

# Check if version is installed
if isUnityImprobablePathPresent; then
    exit 0
fi

# Setup unity_downloader
markStartOfBlock "Installing unity_downloader"
mkdir -p "${GOPATH}"
go get -v -d "${UNITY_PACKAGE}"
markEndOfBlock "Installing unity_downloader"

# Run unity_downloader with platform specific modules
markStartOfBlock "Download Unity"
go run "${GOPATH}/src/${UNITY_PACKAGE}/main.go" "${UNITY_VERSION}" "${UNITY_ROOT}" "--modules" ${MODULES}
markEndOfBlock "Download Unity"

markEndOfBlock "$0"
