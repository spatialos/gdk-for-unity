#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
    set -x
fi

cd "$(dirname "$0")/../"

source .shared-ci/scripts/pinned-tools.sh

ACCELERATOR_ARGS=$(getAcceleratorArgs)

PROJECT_DIR="$(pwd)"
XML_RESULTS_DIR="${PROJECT_DIR}/logs/nunit"
JSON_RESULTS_DIR="${PROJECT_DIR}/perftest-results"
CONFIG_FILE="ci/perftest-configs.json"

mkdir -p "${XML_RESULTS_DIR}"
mkdir -p "${JSON_RESULTS_DIR}"

TEST_SETTINGS_DIR=$(mktemp -d "${TMPDIR:-/tmp}/XXXXXXXXX")

NUM_CONFIGS=$(jq '. | length' ${CONFIG_FILE})

JOB_ID=${BUILDKITE_PARALLEL_JOB:-0}
NUM_JOBS=${BUILDKITE_PARALLEL_JOB_COUNT:-1}

# `parallelism` must be 4 else all jobs are done sequentially.
if [[ ${NUM_JOBS} != 4 ]]; then
    JOB_ID=0
    NUM_JOBS=1
fi

if isMacOS; then
    PLAYMODE_PLATFORM="StandaloneOSX"
elif isLinux; then
    PLAYMODE_PLATFORM="StandaloneLinux64"
else
    PLAYMODE_PLATFORM="StandaloneWindows64"
fi

function main {
    pushd "workers/unity"
        traceStart "Generate csproj & sln files :csharp:"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -nographics \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -quit \
                -logfile "${PROJECT_DIR}/logs/generate-csproj-sln-files.log" \
                -executeMethod UnityEditor.SyncVS.SyncSolution \
                "${ACCELERATOR_ARGS}"
        traceEnd
    popd

    for config in `seq ${JOB_ID} ${NUM_JOBS} $((${NUM_CONFIGS}-1))`
    do
        runTests $config
    done

    pushd "workers/unity"
        traceStart "Parsing XML Test Results"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -nographics \
                -quit \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                "${ACCELERATOR_ARGS}" \
                -logfile "${PROJECT_DIR}/logs/results-parsing.log" \
                -executeMethod "Improbable.Gdk.TestUtils.Editor.PerformanceTestRunParser.Parse" \
                -xmlResultsDirectory "${XML_RESULTS_DIR}" \
                -jsonOutputDirectory "${JSON_RESULTS_DIR}"
        traceEnd
    popd

    cleanUnity "$(pwd)/workers/unity"
}

function runTests {
    local configId=${1}

    local platform=$(jq -r .[${configId}].platform ${CONFIG_FILE})
    local category=$(jq -r .[${configId}].category ${CONFIG_FILE})
    local burst=$(jq -r .[${configId}].burst ${CONFIG_FILE})
    local apiProfile=$(jq -r .[${configId}].apiProfile ${CONFIG_FILE})
    local scriptingBackend=$(jq -r .[${configId}].scriptingBackend ${CONFIG_FILE})

    local args=()

    if [[ "${platform}" == "Editmode" ]]; then
        args+=("-runEditorTests")
    else
        platform=${PLAYMODE_PLATFORM}
        args+=("-runTests -testPlatform ${platform} -buildTarget ${platform}")
    fi

    if [[ "${burst}" == "disabled" ]]; then
        args+=("--burst-disable-compilation")
    fi

    TEST_SETTINGS_PATH="${TEST_SETTINGS_DIR}/${apiProfile}-${scriptingBackend}.json"
    TEST_SETTINGS_JSON=$( jq -n \
                  --arg apiProfile "${apiProfile}" \
                  --arg scriptingBackend "${scriptingBackend}" \
                  '{apiProfile: $apiProfile, scriptingBackend: $scriptingBackend}' \
                  > ${TEST_SETTINGS_PATH})

    traceStart "${platform}: ${burst} ${apiProfile} ${scriptingBackend}"
        pushd "workers/unity"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -nographics \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                "${ACCELERATOR_ARGS}" \
                -logfile "${PROJECT_DIR}/logs/${platform}-${burst}-${apiProfile}-${scriptingBackend}-perftest-run.log" \
                -testResults "${XML_RESULTS_DIR}/${platform}-${burst}-${apiProfile}-${scriptingBackend}-perftest-results.xml" \
                -testCategory "${category}" \
                -testSettingsFile "${TEST_SETTINGS_PATH}" \
                ${args[@]}
        popd
    traceEnd
}

main
