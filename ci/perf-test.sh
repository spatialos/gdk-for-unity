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

# `parallelism` must be 4 else all jobs are done sequentially.
if [[ ${BUILDKITE_PARALLEL_JOB_COUNT:-0} == 4 ]]; then
    JOB_ID=${BUILDKITE_PARALLEL_JOB}
fi

if isMacOS; then
    PLAYMODE_PLATFORM="StandaloneOSX"
elif isLinux; then
    PLAYMODE_PLATFORM="StandaloneLinux64"
else
    PLAYMODE_PLATFORM="StandaloneWindows64"
fi

function runTests {
    local platform=$1
    local category=$2
    local burst=$3
    local apiProfile=$4

    local scriptingBackend="Mono2x"
    local args=()

    if [[ "${platform}" == "editmode" ]]; then
        args+=("-runEditorTests")
    else
        scriptingBackend=$5
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

targetId=0

for burst in burst-default burst-disabled
do
    for apiProfile in NET_Standard_2_0 NET_4_6
    do
        if [[ ${targetId} == ${JOB_ID:-$targetId} ]]; then
            runTests "editmode" "Performance" ${burst} ${apiProfile}

            for scriptingBackend in Mono2x IL2CPP WinRTDotNET
            do
                runTests ${PLAYMODE_PLATFORM} "Performance" ${burst} ${apiProfile} ${scriptingBackend}
            done
        fi

        ((targetId+=1))
    done
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
