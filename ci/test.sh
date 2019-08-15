#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

cd "$(dirname "$0")/../"

source .shared-ci/scripts/pinned-tools.sh

PROJECT_DIR="$(pwd)"
mkdir -p "${PROJECT_DIR}/logs/"

CODE_GEN_LIB_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/code-gen-lib-test-results.xml"
CODE_GENERATOR_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/code-generator-test-results.xml"
EDITMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/editmode-test-results.xml"
PLAYMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/playmode-test-results.xml"
TEST_PROJECT_EDITMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/test-project-editmode-test-results.xml"
TEST_PROJECT_PLAYMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/test-project-playmode-test-results.xml"

echo "--- Testing Code Generator :gear:"

dotnet test \
    --logger:"nunit;LogFilePath=${CODE_GEN_LIB_TEST_RESULTS_FILE}" \
    workers/unity/Packages/io.improbable.gdk.tools/.CodeGenerator/CodeGeneration/CodeGeneration.csproj

dotnet test \
    --logger:"nunit;LogFilePath=${CODE_GENERATOR_TEST_RESULTS_FILE}" \
    workers/unity/Packages/io.improbable.gdk.tools/.CodeGenerator/GdkCodeGenerator/GdkCodeGenerator.csproj

echo "--- Testing Unity: Editmode :writing_hand:"

pushd "workers/unity"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/workers/unity" \
        -runEditorTests \
        -logfile "${PROJECT_DIR}/logs/unity-editmode-test-run.log" \
        -editorTestsResultFile "${EDITMODE_TEST_RESULTS_FILE}"
popd

cleanUnity "$(pwd)/workers/unity"

echo "--- Testing Unity: Playmode :joystick:"

pushd "workers/unity"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/workers/unity" \
        -runTests \
        -testPlatform playmode \
        -logfile "${PROJECT_DIR}/logs/unity-playmode-test-run.log" \
        -testResults "${PLAYMODE_TEST_RESULTS_FILE}"
popd

echo "--- Testing Unity: Test Project Editmode :microscope:"

pushd "test-project"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/test-project" \
        -runEditorTests \
        -logfile "${PROJECT_DIR}/logs/test-project-editmode-test-run.log" \
        -editorTestsResultFile "${TEST_PROJECT_EDITMODE_TEST_RESULTS_FILE}"
popd

echo "--- Testing Unity: Test Project Playmode :joystick:"

pushd "test-project"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/test-project" \
        -runTests \
        -testPlatform playmode \
        -logfile "${PROJECT_DIR}/logs/test-project-playmode-test-run.log" \
        -testResults "${TEST_PROJECT_PLAYMODE_TEST_RESULTS_FILE}"
popd
