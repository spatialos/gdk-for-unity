#!/usr/bin/env bash
set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

echo "Building for: ${WORKER_TYPE} ${BUILD_TARGET} ${SCRIPTING_TYPE}"

cd "$(dirname "$0")/../"

ci/bootstrap.sh
source ".shared-ci/scripts/pinned-tools.sh"

if isDocsBranch; then
    exit 0
fi

if [[ ${WORKER_TYPE} == "AndroidClient" ]]; then
    .shared-ci/scripts/prepare-unity-mobile.sh "$(pwd)/logs/PrepareUnityMobile.log"
fi

if [[ ${WORKER_TYPE} == "iOSClient" ]]; then
    if ! isMacOS; then
        echo "I can't build for iOS!"
        exit 0
    fi
fi

LOG_LOCATION="$(pwd)/logs/${WORKER_TYPE}-${BUILD_TARGET}-${SCRIPTING_TYPE}.log"

.shared-ci/scripts/build.sh "workers/unity" ${WORKER_TYPE} ${BUILD_TARGET} ${SCRIPTING_TYPE} "${LOG_LOCATION}"
spatial prepare-for-run
