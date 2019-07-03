#!/usr/bin/env bash
set -u -x -o pipefail

cd "$(dirname "$0")/../"

ci/bootstrap.sh

source .shared-ci/scripts/pinned-tools.sh

if isDocsBranch; then
    exit 0
fi

echo "$0"

#####
# Setup variables
#####
echo "Setup variables"
PROJECT_DIR="$(pwd)"
mkdir -p "${PROJECT_DIR}/logs/"

CODE_GEN_LIB_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/code-gen-lib-test-results.xml"
CODE_GENERATOR_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/code-generator-test-results.xml"
EDITMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/editmode-test-results.xml"
PLAYMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/playmode-test-results.xml"
TEST_PROJECT_EDITMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/test-project-editmode-test-results.xml"

rm  "${CODE_GENERATOR_TEST_RESULTS_FILE}" \
    "${EDITMODE_TEST_RESULTS_FILE}" \
    "${PLAYMODE_TEST_RESULTS_FILE}"

cleanUnity "$(pwd)/workers/unity"
cleanUnity "$(pwd)/test-project"

echo "Code Generator Testing"

dotnet test --logger:"nunit;LogFilePath=${CODE_GEN_LIB_TEST_RESULTS_FILE}" workers/unity/Packages/com.improbable.gdk.tools/.CodeGenerator/CodeGeneration/CodeGeneration.csproj
CODE_GEN_LIB_TEST_RESULT=$?

dotnet test --logger:"nunit;LogFilePath=${CODE_GENERATOR_TEST_RESULTS_FILE}" workers/unity/Packages/com.improbable.gdk.tools/.CodeGenerator/GdkCodeGenerator/GdkCodeGenerator.csproj
CODE_GENERATOR_TEST_RESULT=$?

echo "Editmode Testing"

pushd "workers/unity"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/workers/unity" \
        -runTests \
        -testPlatform editmode \
        -logfile "${PROJECT_DIR}/logs/unity-editmode-test-run.log" \
        -testResults "${EDITMODE_TEST_RESULTS_FILE}"

    EDITMODE_TEST_RESULT=$?
popd

cleanUnity "$(pwd)/workers/unity"

echo "Playmode Testing"

pushd "workers/unity"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/workers/unity" \
        -runTests \
        -testPlatform playmode \
        -logfile "${PROJECT_DIR}/logs/unity-playmode-test-run.log" \
        -testResults "${PLAYMODE_TEST_RESULTS_FILE}"

    PLAYMODE_TEST_RESULT=$?
popd

pushd "test-project"
echo "Generated Code Testing"
    dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
        -batchmode \
        -projectPath "${PROJECT_DIR}/test-project" \
        -runTests \
        -testPlatform editmode \
        -logfile "${PROJECT_DIR}/logs/test-project-editmode-test-run.log" \
        -testResults "${TEST_PROJECT_EDITMODE_TEST_RESULTS_FILE}"

    TEST_PROJECT_EDITMODE_TEST_RESULT=$?
popd

if [ $CODE_GEN_LIB_TEST_RESULT -ne 0 ]; then
    >&2 echo "Code Generator Tests failed. Please check the file ${CODE_GEN_LIB_TEST_RESULTS_FILE} for more information."
fi

if [ $CODE_GENERATOR_TEST_RESULT -ne 0 ]; then
    >&2 echo "Code Generator Tests failed. Please check the file ${CODE_GENERATOR_TEST_RESULTS_FILE} for more information."
fi

if [ $EDITMODE_TEST_RESULT -ne 0 ]; then
    >&2 echo "Editmode Tests failed. Please check the file ${EDITMODE_TEST_RESULTS_FILE} for more information."
fi

if [ $PLAYMODE_TEST_RESULT -ne 0 ]; then
    >&2 echo "Playmode Tests failed. Please check the file ${PLAYMODE_TEST_RESULTS_FILE} for more information."
fi

if [ $TEST_PROJECT_EDITMODE_TEST_RESULT -ne 0 ]; then
    >&2 echo "Test Project Editmode Tests failed. Please check the file ${TEST_PROJECT_EDITMODE_TEST_RESULTS_FILE} for more information."
fi

cleanUnity "$(pwd)/workers/unity"
cleanUnity "$(pwd)/test-project"

if [ $EDITMODE_TEST_RESULT -ne 0 ] || \
   [ $PLAYMODE_TEST_RESULT -ne 0 ] || \
   [ $CODE_GENERATOR_TEST_RESULT -ne 0 ] || \
   [ $TEST_PROJECT_EDITMODE_TEST_RESULT -ne 0 ] || \
   [ $CODE_GEN_LIB_TEST_RESULT -ne 0 ]
then
    >&2 echo "Tests failed! See above for more information."
    exit 1
fi

echo "All tests passed!"
