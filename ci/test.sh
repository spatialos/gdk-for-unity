#!/usr/bin/env bash
set -u -x -o pipefail

cd "$(dirname "$0")/../"

source ci/includes/pinned-tools.sh
source ci/includes/profiling.sh

markStartOfBlock "$0"

#####
# Setup variables
#####
markStartOfBlock "Setup variables"
PROJECT_DIR="$(pwd)"
mkdir -p "${PROJECT_DIR}/logs/"

TOOLS_TEST_RESULTS_FILES="${PROJECT_DIR}/logs/tools-test-results.xml"
CODE_GENERATOR_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/code-generator-test-results.xml"
EDITMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/editmode-test-results.xml"
PLAYMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/playmode-test-results.xml"

markEndOfBlock "Setup variables"

cleanUnity

markStartOfBlock "Tools Testing"

dotnet test --logger:"nunit;LogFilePath=${TOOLS_TEST_RESULTS_FILES}" "${PROJECT_DIR}/tools/DocsLinter/DocsLinter.csproj"
TOOLS_TEST_RESULT=$?

markEndOfBlock "Tools Testing"

markStartOfBlock "Code Generator Testing"

dotnet test --logger:"nunit;LogFilePath=${CODE_GENERATOR_TEST_RESULTS_FILE}" "${PROJECT_DIR}/code_generator/GdkCodeGenerator/GdkCodeGenerator.csproj"
CODE_GENERATOR_TEST_RESULT=$?

markEndOfBlock "Code Generator Testing"

markStartOfBlock "Editmode Testing"

pushd "workers/unity"
  dotnet run -p "${PROJECT_DIR}/tools/RunUnity/RunUnity.csproj" -- \
    -batchmode \
    -projectPath "${PROJECT_DIR}/workers/unity" \
    -runTests \
    -testPlatform editmode \
    -logfile "${PROJECT_DIR}/logs/unity-editmode-test-run.log" \
    -testResults "${EDITMODE_TEST_RESULTS_FILE}"

    EDITMODE_TEST_RESULT=$?
popd

markEndOfBlock "Editmode Testing"

cleanUnity

markStartOfBlock "Playmode Testing"

pushd "workers/unity"
  dotnet run -p "${PROJECT_DIR}/tools/RunUnity/RunUnity.csproj" -- \
    -batchmode \
    -projectPath "${PROJECT_DIR}/workers/unity" \
    -runTests \
    -testPlatform playmode \
    -logfile "${PROJECT_DIR}/logs/unity-playmode-test-run.log" \
    -testResults "${PLAYMODE_TEST_RESULTS_FILE}"

    PLAYMODE_TEST_RESULT=$?
popd

markEndOfBlock "Playmode Testing"

if [ $TOOLS_TEST_RESULT -ne 0 ]; then
    >&2 echo "Tools Tests failed. Please check the file ${TOOLS_TEST_RESULTS_FILES} for more information."
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

markEndOfBlock "$0"

cleanUnity

if [ $EDITMODE_TEST_RESULT -ne 0 ] || \
   [ $PLAYMODE_TEST_RESULT -ne 0 ] || \
   [ $CODE_GENERATOR_TEST_RESULT -ne 0 ] || \
   [ $TOOLS_TEST_RESULT -ne 0 ]
then
    >&2 echo "Tests failed! See above for more information."
    exit 1
fi

echo "All tests passed!"
