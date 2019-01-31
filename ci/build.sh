#!/usr/bin/env bash

set -e -u -o -x pipefail

cd "$(dirname "$0")/../"

ci/bootstrap.sh
source ".shared-ci/scripts/pinned-tools.sh"

if isDocsBranch; then
    exit 0
fi

WORKER_TYPE=$1
BUILD_TARGET=$2
SCRIPTING_TYPE=$3

if [[ ${WORKER_TYPE} == "AndroidClient" ]]; then
    .shared-ci/scripts/prepare-unity-mobile.sh "$(pwd)/logs/PrepareUnityMobile.log"
fi

if [[ ${WORKER_TYPE} == "iOSClient" ]]; then
    if !isMacOs; then
        echo "I can't build for iOS!"
        exit 0
    fi
fi

LOG_LOCATION="$(pwd)/logs/${WORKER_TYPE}-${BUILD_TARGET}-${SCRIPTING_TYPE}.log"

./shared-ci/scripts/build.sh "workers/unity" ${WORKER_TYPE} ${BUILD_TARGET} ${SCRIPTING_TYPE} "${LOG_LOCATION}"
