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
COVERAGE_RESULTS_PATH="${PROJECT_DIR}/logs/coverage-results"

echo "--- Testing Code Generator :gear:"

dotnet test \
    --logger:"nunit;LogFilePath=${TEST_RESULTS_DIR}/code-gen-lib-test-results.xml" \
    workers/unity/Packages/io.improbable.gdk.tools/.CodeGenTemplate/CodeGenerationLib/CodeGenerationLib.csproj

#dotnet output does not end with a newline, we force one here to fix buildkite output.
echo ""
echo "--- Testing Unity: Editmode :writing_hand:"

pushd "workers/unity"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -enableCodeCoverage \
        -coverageResultsPath "${COVERAGE_RESULTS_PATH}/playground" \
        -coverageOptions "${COVERAGE_OPTIONS}" \
        -projectPath "${PROJECT_DIR}/workers/unity" \
        -runEditorTests \
        -logfile "${PROJECT_DIR}/logs/unity-editmode-test-run.log" \
        -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-test-results.xml" \
        "${ACCELERATOR_ARGS}"
popd

echo "--- Testing Unity: Playmode :joystick:"

pushd "workers/unity"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -enableCodeCoverage \
        -coverageResultsPath "${COVERAGE_RESULTS_PATH}/playground" \
        -coverageOptions "${COVERAGE_OPTIONS}" \
        -projectPath "${PROJECT_DIR}/workers/unity" \
        -runTests \
        -testPlatform playmode \
        -logfile "${PROJECT_DIR}/logs/playmode-test-run.log" \
        -testResults "${TEST_RESULTS_DIR}/playmode-test-results.xml" \
        "${ACCELERATOR_ARGS}"
popd

cleanUnity "$(pwd)/workers/unity"
