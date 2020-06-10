#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
    set -x
fi

cd "$(dirname "$0")/../"

source .shared-ci/scripts/pinned-tools.sh

ACCELERATOR_ARGS=$(getAcceleratorArgs)

PROJECT_DIR="$(pwd)"
TEST_RESULTS_DIR="${PROJECT_DIR}/logs/nunit"
mkdir -p "${TEST_RESULTS_DIR}"

traceStart "Testing Unity: Editmode :writing_hand:"
    pushd "workers/unity"
        dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
            -batchmode \
            -projectPath "${PROJECT_DIR}/workers/unity" \
            -runEditorTests \
            -logfile "${PROJECT_DIR}/logs/editmode-perftest-run.log" \
            -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-perftest-results.xml" \
            "${ACCELERATOR_ARGS}"
    popd
traceEnd

traceStart "Testing Unity: Playmode :joystick:"
    pushd "workers/unity"
        dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
            -batchmode \
            -projectPath "${PROJECT_DIR}/workers/unity" \
            -runTests \
            -testPlatform playmode \
            -logfile "${PROJECT_DIR}/logs/playmode-perftest-run.log" \
            -testResults "${TEST_RESULTS_DIR}/playmode-perftest-results.xml" \
            "${ACCELERATOR_ARGS}"
    popd
traceEnd

cleanUnity "$(pwd)/workers/unity"
