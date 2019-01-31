#!/usr/bin/env bash

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

# Get shared CI and prepare Unity
ci/bootstrap.sh
.shared-ci/scripts/prepare-unity.sh

source ".shared-ci/scripts/pinned-tools.sh"

if ! isMacOS; then
    echo "iOS can only be built on macOS"
    exit 1
fi

.shared-ci/scripts/build.sh "workers/unity" iOSClient local il2cpp "$(pwd)/logs/iOSClientBuild.log"

cp -R .shared-ci/fastlane workers/unity/build/worker/iOSClient@iOS/iOSClient@iOS
pushd workers/unity/build/worker/iOSClient@iOS/iOSClient@iOS
    fastlane dev
popd
