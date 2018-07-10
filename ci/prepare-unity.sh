#!/usr/bin/env bash

### This script is used to setup Improbable's internal build machines.
### If you don't work at Improbable, this may be interesting as a guide to what software versions we use for our
### automation, but not much more than that.

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

# Only run inside TeamCity
if [ -z "${TEAMCITY_CAPTURE_ENV+x}" ]; then
    exit 0
fi

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

if isWindows; then
    MODULES="linux,mac-mono,ios,android"
elif isMacOS; then
    MODULES="linux,windows,ios,android"
elif isLinux; then
    echo "Building is not supported on linux"
    exit 1
fi

GOPATH="$(pwd)/go"
export GOPATH

UNITY_PACKAGE="github.com/improbable/unity_downloader"
UNITY_PINNED_HASH=$(cat "workers/unity/ProjectSettings/ProjectVersionHash.txt")

markStartOfBlock "$0"

# Check if version is installed
if isUnityImprobablePathPresent; then
    echo "Unity ${UNITY_VERSION} is already installed."
    exit 0
fi

# Setup unity_downloader
markStartOfBlock "Installing unity_downloader"
git clone "git@github.com:improbable/unity_downloader.git" "go/src/${UNITY_PACKAGE}"
go get -v -d "${UNITY_PACKAGE}"
markEndOfBlock "Installing unity_downloader"

# Run unity_downloader with platform specific modules
markStartOfBlock "Download Unity"
pushd "${GOPATH}/src/${UNITY_PACKAGE}"
go run main.go install "${UNITY_VERSION}" "${UNITY_PINNED_HASH}" "${IMPROBABLE_UNITY_ROOT}" "--modules" ${MODULES}
popd
markEndOfBlock "Download Unity"

markEndOfBlock "$0"
