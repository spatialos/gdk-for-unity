#!/usr/bin/env bash

set -e -u -o -x pipefail

cd "$(dirname "$0")/../"

function isDocsBranch() {
  if [[ -n "${BUILDKITE-}" ]]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
  else
    BRANCH="${BUILDKITE_BRANCH}"
  fi

  if [[ "${BRANCH}" == docs/* ]]; then
    return 0
  fi
  return 1
}

if isDocsBranch; then
    exit 0
fi


# Get shared CI and prepare Unity
ci/bootstrap.sh
.shared-ci/scripts/prepare-unity.sh
.shared-ci/scripts/prepare-unity-mobile.sh "$(pwd)/logs/PrepareUnityMobile.log"

source ".shared-ci/scripts/pinned-tools.sh"

ci/test.sh
.shared-ci/scripts/build.sh "workers/unity" AndroidClient local mono "$(pwd)/logs/AndroidClientBuild.log"
.shared-ci/scripts/build.sh "workers/unity" UnityClient local il2cpp "$(pwd)/logs/UnityClientBuild-il2cpp.log"
.shared-ci/scripts/build.sh "workers/unity" UnityClient cloud mono "$(pwd)/logs/UnityClientBuild-mono.log"
.shared-ci/scripts/build.sh "workers/unity" UnityGameLogic cloud mono "$(pwd)/logs/UnityGameLogicBuild.log"

if isMacOS; then
  .shared-ci/scripts/build.sh "workers/unity" iOSClient local il2cpp "$(pwd)/logs/iOSClientBuild.log"
fi
