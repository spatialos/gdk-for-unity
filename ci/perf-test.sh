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
MONO_BACKEND="${TEST_SETTINGS_DIR}/Mono.json"
IL2CPP_BACKEND="${TEST_SETTINGS_DIR}/IL2CPP.json"

traceStart "Testing Unity: Editmode :writing_hand:"
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
                "${ACCELERATOR_ARGS}"
        traceEnd
    popd
traceEnd

traceStart "Testing Unity: Playmode :joystick:"
    pushd "workers/unity"
        traceStart "Burst default - Mono backend"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/playmode-burst-default-mono-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burst-default-mono-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $MONO_BACKEND
        traceEnd

        traceStart "Burst default - IL2CPP backend"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/playmode-burst-default-il2cpp-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burst-default-il2cpp-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $IL2CPP_BACKEND
        traceEnd

        traceStart "Burst disabled - Mono backend"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/playmode-burst-disabled-mono-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burst-disabled-mono-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $MONO_BACKEND \
                --burst-disable-compilation
        traceEnd

        traceStart "Burst disabled - IL2CPP backend"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/playmode-burst-disabled-il2cpp-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burst-disabled-il2cpp-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $IL2CPP_BACKEND \
                --burst-disable-compilation
        traceEnd
    popd
traceEnd

cleanUnity "$(pwd)/workers/unity"
