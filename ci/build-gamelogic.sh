#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

source .shared-ci/scripts/pinned-tools.sh
source .shared-ci/scripts/profiling.sh

UNITY_PROJECT_DIR="$(pwd)/workers/unity"

markStartOfBlock "$0"

markStartOfBlock "Building UnityGameLogic"

TARGET=${1:-cloud}

# Back-compat: we need to run this to generate the bridge configurations.
spatial build build-config UnityGameLogic

pushd "${UNITY_PROJECT_DIR}"
    dotnet run -p ../../.shared-ci/tools/RunUnity/RunUnity.csproj -- \
        -projectPath "${UNITY_PROJECT_DIR}" \
        -batchmode \
        -quit \
        -logfile "$(pwd)/../../logs/UnityGameLogicBuild.log" \
        -executeMethod "Improbable.Gdk.BuildSystem.WorkerBuilder.Build" \
        +buildWorkerTypes "UnityGameLogic" \
        +buildTarget "${TARGET}"
popd

markEndOfBlock "Building UnityGameLogic"

markEndOfBlock "$0"
