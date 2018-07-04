#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

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
    echo "Unity ${UNITY_VERSION} is already installed"
    exit 0
fi

# Setup unity_downloader
markStartOfBlock "Installing unity_downloader"
go get -v -d "${UNITY_PACKAGE}"
markEndOfBlock "Installing unity_downloader"

# Run unity_downloader with platform specific modules
markStartOfBlock "Download Unity"
go run "${GOPATH}/src/${UNITY_PACKAGE}/main.go" "${UNITY_VERSION}" "${UNITY_ROOT}" "--modules" ${MODULES}
markEndOfBlock "Download Unity"

markEndOfBlock "$0"
