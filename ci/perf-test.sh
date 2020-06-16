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

TEST_SETTINGS_DIR="${PROJECT_DIR}/workers/unity/Packages/io.improbable.gdk.testutils/TestSettings"

function runPlaymodeTests {
    local burst=$1
    local scriptingBackend=$2
    local apiProfile=$3
    local category=$4
    local platform=$5

    local args=()
    args+=("-batchmode -runTests")
    args+=("-projectPath ${PROJECT_DIR}/workers/unity ")
    args+=("-logfile ${PROJECT_DIR}/logs/playmode-${burst}-${scriptingBackend}-${apiProfile}-perftest-run.log")
    args+=("-testResults ${TEST_RESULTS_DIR}/playmode-${burst}-${scriptingBackend}-${apiProfile}-perftest-results.xml")
    args+=("-testSettingsFile ${TEST_SETTINGS_DIR}/${scriptingBackend}-${apiProfile}.json")

    if [[ "${burst}" == "burst-disabled" ]]; then
        args+=('--burst-disable-compilation')
    fi

    traceStart "${burst} ${scriptingBackend} ${apiProfile}"
        dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
            "${ACCELERATOR_ARGS}" \
            -testCategory "${category}" \
            -testPlatform "${platform}" \
            "${args[@]}"
    traceEnd
}

traceStart "Performance Testing: Editmode :writing_hand:"
    pushd "workers/unity"
        traceStart "Burst default"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runEditorTests \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/editmode-burst-default-perftest-run.log" \
                -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-burst-default-perftest-results.xml" \
                "${ACCELERATOR_ARGS}"
        traceEnd

        traceStart "Burst disabled"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runEditorTests \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/editmode-burst-disabled-perftest-run.log" \
                -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-burst-disabled-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                --burst-disable-compilation
        traceEnd
    popd
traceEnd

traceStart "Performance Testing: Playmode :joystick:"
    pushd "workers/unity"
        for burst in burst-default burst-disabled
        do
            for scriptingBackend in mono il2cpp winrt
            do
                for apiProfile in dotnet-std-2 dotnet-4
                do
                    runPlaymodeTests $burst $scriptingBackend $apiProfile "Performance" "StandaloneWindows64"
                done
            done
        done
    popd
traceEnd

cleanUnity "$(pwd)/workers/unity"
