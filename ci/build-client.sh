#!/usr/bin/env bash
set -e -u -o pipefail
if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

UNITY_PROJECT_DIR="$(pwd)/workers/unity"

markStartOfBlock "$0"

markStartOfBlock "Building UnityClient"

TARGET=${1:-local}

# Back-compat: we need to run this to generate the bridge configurations.
spatial build build-config UnityClient

pushd "${UNITY_PROJECT_DIR}"
    dotnet run -p ../../tools/RunUnity/RunUnity.csproj -- \
        -projectPath "${UNITY_PROJECT_DIR}" \
        -batchmode \
        -quit \
        -logfile "$(pwd)/../../logs/UnityClientBuild.log" \
        -executeMethod "Improbable.Gdk.Legacy.BuildSystem.WorkerBuilder.Build" \
        +buildWorkerTypes "UnityClient" \
        +buildTarget "${TARGET}"
popd

markEndOfBlock "Building UnityClient"

markEndOfBlock "$0"
