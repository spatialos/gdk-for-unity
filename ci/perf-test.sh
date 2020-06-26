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

mkdir -p "${XML_RESULTS_DIR}"
mkdir -p "${JSON_RESULTS_DIR}"

TEST_SETTINGS_DIR=$(mktemp -d "${TMPDIR:-/tmp}/XXXXXXXXX")

declare -A CONFIGS=()
NUM_CONFIGS=0

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
    configureTargets

    for config in `seq ${JOB_ID} ${NUM_JOBS} $((${NUM_CONFIGS}-1))`
    do
        runTests $config
    done

    traceStart "Parsing XML Test Results"
        pushd "workers/unity"
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
        popd
    traceEnd

    cleanUnity "$(pwd)/workers/unity"
}

function configureTargets {
    # Editmode tests
    addConfig editmode Performance burst-default NET_Standard_2_0 Mono2x
    addConfig editmode Performance burst-default NET_4_6 Mono2x
    addConfig editmode Performance burst-disabled NET_Standard_2_0 Mono2x
    addConfig editmode Performance burst-disabled NET_4_6 Mono2x

    # Mono2x backend
    addConfig ${PLAYMODE_PLATFORM} Performance burst-default NET_Standard_2_0 Mono2x
    addConfig ${PLAYMODE_PLATFORM} Performance burst-default NET_4_6 Mono2x
    addConfig ${PLAYMODE_PLATFORM} Performance burst-disabled NET_Standard_2_0 Mono2x
    addConfig ${PLAYMODE_PLATFORM} Performance burst-disabled NET_4_6 Mono2x

    # IL2CPP backend
    addConfig ${PLAYMODE_PLATFORM} Performance burst-default NET_Standard_2_0 IL2CPP
    addConfig ${PLAYMODE_PLATFORM} Performance burst-default NET_4_6 IL2CPP
    addConfig ${PLAYMODE_PLATFORM} Performance burst-disabled NET_Standard_2_0 IL2CPP
    addConfig ${PLAYMODE_PLATFORM} Performance burst-disabled NET_4_6 IL2CPP

    # WinRTDotNET backend
    addConfig ${PLAYMODE_PLATFORM} Performance burst-default NET_Standard_2_0 WinRTDotNET
    addConfig ${PLAYMODE_PLATFORM} Performance burst-default NET_4_6 WinRTDotNET
    addConfig ${PLAYMODE_PLATFORM} Performance burst-disabled NET_Standard_2_0 WinRTDotNET
    addConfig ${PLAYMODE_PLATFORM} Performance burst-disabled NET_4_6 WinRTDotNET
}

function addConfig {
    CONFIGS["${NUM_CONFIGS}, platform"]=${1}
    CONFIGS["${NUM_CONFIGS}, category"]=${2}
    CONFIGS["${NUM_CONFIGS}, burst"]=${3}
    CONFIGS["${NUM_CONFIGS}, apiProfile"]=${4}
    CONFIGS["${NUM_CONFIGS}, scriptingBackend"]=${5}

    ((NUM_CONFIGS+=1))
}

function runTests {
    local configId=${1}

    local platform="${CONFIGS["${configId}, platform"]}"
    local category="${CONFIGS["${configId}, category"]}"
    local burst="${CONFIGS["${configId}, burst"]}"
    local apiProfile="${CONFIGS["${configId}, apiProfile"]}"
    local scriptingBackend="${CONFIGS["${configId}, scriptingBackend"]}"

    local args=()

    if [[ "${platform}" == "editmode" ]]; then
        args+=("-runEditorTests")
    else
        args+=("-runTests -testPlatform ${platform} -buildTarget ${platform}")
    fi

    if [[ "${burst}" == "burst-disabled" ]]; then
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
