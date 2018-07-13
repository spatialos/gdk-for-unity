#!/usr/bin/env bash

### This script is used to setup Improbable's internal build machines.
### If you don't work at Improbable, this may be interesting as a guide to what software versions we use for our
### automation, but not much more than that.

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

# Only run inside TeamCity
if [ -z "${TEAMCITY_CAPTURE_ENV+x}" ]; then
  exit 0
fi

markStartOfBlock "$0"

UNITY_PINNED_VERSION=$(grep "m_EditorVersion" "workers/unity/ProjectSettings/ProjectVersion.txt" | cut -d ' ' -f2)
UNITY_PINNED_HASH=$(cat "workers/unity/ProjectSettings/ProjectVersionHash.txt")
IMPROBABLE_UNITY_ROOT="C:/Unity"
IMPROBABLE_UNITY_VERSIONED="$IMPROBABLE_UNITY_ROOT/$UNITY_PINNED_VERSION"

if [ -d "$IMPROBABLE_UNITY_VERSIONED" ]; then
  echo "Unity is already installed in:" $IMPROBABLE_UNITY_VERSIONED
  exit 0
else
  echo "Could not find Unity installation."
fi

UNITY_PACKAGE="github.com/improbable/unity_downloader"
GOPATH="$(pwd)/go"
export GOPATH

if isWindows; then
  MODULES="linux,mac-mono,ios,android"
elif isMacOS; then
  MODULES="linux,windows-mono,ios,android"
elif isLinux; then
  echo "Building is not supported on linux."
  exit 1
fi

markStartOfBlock "Installing unity_downloader"

rm -rf "go/src/${UNITY_PACKAGE}"
git clone "git@github.com:improbable/unity_downloader.git" "go/src/${UNITY_PACKAGE}"
go get -v -d "${UNITY_PACKAGE}"

markEndOfBlock "Installing unity_downloader"

markStartOfBlock "Download Unity"

pushd "${GOPATH}/src/${UNITY_PACKAGE}"
  go run main.go install "${UNITY_PINNED_VERSION}" "${UNITY_PINNED_HASH}" "${IMPROBABLE_UNITY_ROOT}" "--modules" "${MODULES}"
popd

markEndOfBlock "Download Unity"

markEndOfBlock "$0"
