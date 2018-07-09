#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

UNITY_PROJECT_DIR="$(pwd)/workers/unity"

markStartOfBlock "$0"

markStartOfBlock "Building UnityClient"

# Back-compat: we need to run this to generate the bridge configurations.
spatial build build-config UnityClient

"${UNITY_EXE}" -projectPath "${UNITY_PROJECT_DIR}" \
    -batchmode \
    -quit \
    -logfile "$(pwd)/build/build_logs/UnityClientBuild.log" \
    -executeMethod "Improbable.Gdk.Legacy.BuildSystem.WorkerBuilder.Build" \
    +buildWorkerTypes "UnityClient" \
    +buildTarget "local"

markEndOfBlock "Building UnityClient"

markEndOfBlock "$0"
