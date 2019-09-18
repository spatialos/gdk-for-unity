#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

cd "$(dirname "$0")/../"

source .shared-ci/scripts/pinned-tools.sh

PROJECT_DIR="$(pwd)"
TEST_RESULTS_DIR="${PROJECT_DIR}/logs/nunit"
mkdir -p "${TEST_RESULTS_DIR}"

# echo "--- Testing Code Generator :gear:"

# dotnet test \
#     --logger:"nunit;LogFilePath=${TEST_RESULTS_DIR}/code-gen-lib-test-results.xml" \
#     workers/unity/Packages/io.improbable.gdk.tools/.CodeGenTemplate/CodeGenerationLib/CodeGenerationLib.csproj

# echo "--- Testing Unity: Editmode :writing_hand:"

# pushd "workers/unity"
#     dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
#         -batchmode \
#         -projectPath "${PROJECT_DIR}/workers/unity" \
#         -runEditorTests \
#         -logfile "${PROJECT_DIR}/logs/unity-editmode-test-run.log" \
#         -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-test-results.xml"
# popd

# cleanUnity "$(pwd)/workers/unity"

# echo "--- Testing Unity: Playmode :joystick:"

# pushd "workers/unity"
#     dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
#         -batchmode \
#         -projectPath "${PROJECT_DIR}/workers/unity" \
#         -runTests \
#         -testPlatform playmode \
#         -logfile "${PROJECT_DIR}/logs/unity-playmode-test-run.log" \
#         -testResults "${TEST_RESULTS_DIR}/playmode-test-results.xml"
# popd

# echo "--- Testing Unity: Test Project Editmode :microscope:"

# pushd "test-project"
#     dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
#         -batchmode \
#         -projectPath "${PROJECT_DIR}/test-project" \
#         -runEditorTests \
#         -logfile "${PROJECT_DIR}/logs/test-project-editmode-test-run.log" \
#         -editorTestsResultFile "${TEST_RESULTS_DIR}/test-project-editmode-test-results.xml"
# popd

echo "--- Testing Unity: Test Project Playmode :joystick:"

pushd "test-project"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/test-project" \
        -runTests \
        -testPlatform playmode \
        -logfile "${PROJECT_DIR}/logs/test-project-playmode-test-run.log" \
        -testResults "${TEST_RESULTS_DIR}/test-project-playmode-test-results.xml"
popd
