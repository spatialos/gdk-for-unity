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

COVERAGE_OPTIONS="generateHtmlReport\;assemblyFilters:+Improbable.Gdk.*,-Improbable.Gdk.Generated,-Improbable.Gdk.Generated.BuildSystem"
COVERAGE_RESULTS_PATH="${PROJECT_DIR}/workers/unity/logs/coverage-results"

if isMacOS; then
    PLAYMODE_PLATFORM="StandaloneOSX"
elif isLinux; then
    PLAYMODE_PLATFORM="StandaloneLinux64"
else
    PLAYMODE_PLATFORM="StandaloneWindows64"
fi

traceStart "Testing Code Generator :gear:"
    dotnet test \
        --logger:"nunit;LogFilePath=${TEST_RESULTS_DIR}/code-gen-lib-test-results.xml" \
        workers/unity/Packages/io.improbable.gdk.tools/.CodeGenTemplate/CodeGenerationLib/CodeGenerationLib.csproj

    #dotnet output does not end with a newline, we force one here to fix buildkite output.
    echo ""
traceEnd

traceStart "Generate csproj & sln files :csharp:"
    pushd "workers/unity"
        dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
            -batchmode \
            -projectPath "${PROJECT_DIR}/workers/unity" \
            -quit \
            -logfile "${PROJECT_DIR}/logs/generate-csproj-sln-files.log" \
            -executeMethod UnityEditor.SyncVS.SyncSolution \
            "${ACCELERATOR_ARGS}"
    popd
traceEnd

traceStart "Testing Unity: Editmode :writing_hand:"
    pushd "workers/unity"
        dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
            -batchmode \
            -enableCodeCoverage \
            -coverageResultsPath "${COVERAGE_RESULTS_PATH}/playground" \
            -coverageOptions "${COVERAGE_OPTIONS}" \
            -projectPath "${PROJECT_DIR}/workers/unity" \
            -runEditorTests \
            -testCategory "Uncategorized" \
            -logfile "${PROJECT_DIR}/logs/editmode-test-run.log" \
            -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-test-results.xml" \
            "${ACCELERATOR_ARGS}"
    popd
traceEnd

traceStart "Testing Unity: Playmode :joystick:"
    pushd "workers/unity"
        dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
            -batchmode \
            -nographics \
            -enableCodeCoverage \
            -coverageResultsPath "${COVERAGE_RESULTS_PATH}/playground" \
            -coverageOptions "${COVERAGE_OPTIONS}" \
            -projectPath "${PROJECT_DIR}/workers/unity" \
            -runTests \
            -testPlatform ${PLAYMODE_PLATFORM} \
            -buildTarget ${PLAYMODE_PLATFORM} \
            -testCategory "Uncategorized" \
            -logfile "${PROJECT_DIR}/logs/playmode-test-run.log" \
            -testResults "${TEST_RESULTS_DIR}/playmode-test-results.xml" \
            "${ACCELERATOR_ARGS}"
    popd
traceEnd

if ! isMacOS; then
    ci/sonar-scanner.sh
fi

cleanUnity "$(pwd)/workers/unity"
