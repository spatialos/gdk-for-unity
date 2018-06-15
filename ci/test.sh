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

CODE_GENERATOR_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/code-generator-test-results.xml"
EDITMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/editmode-test-results.xml"
PLAYMODE_TEST_RESULTS_FILE="${PROJECT_DIR}/logs/playmode-test-results.xml"

markEndOfBlock "Setup variables"

markStartOfBlock "Code Generator Testing"

${NUNIT3_CONSOLE} code_generator/GdkCodeGenerator.sln --result=${CODE_GENERATOR_TEST_RESULTS_FILE}
CODE_GENERATOR_TEST_RESULT=$?

markEndOfBlock "Code Generator Testing"

markStartOfBlock "Editmode Testing"

rm -rf "${PROJECT_DIR}/workers/unity/Library/"
rm -rf "${PROJECT_DIR}/workers/unity/Temp/"

${UNITY_EXE} \
    -nographics \
    -batchmode \
    -projectPath "${PROJECT_DIR}/workers/unity" \
    -runTests \
    -testPlatform editmode \
    -logfile "${PROJECT_DIR}/logs/unity-editmode-test-run.log" \
    -testResults "${EDITMODE_TEST_RESULTS_FILE}"

EDITMODE_TEST_RESULT=$?

markEndOfBlock "Editmode Testing"

markStartOfBlock "Playmode Testing"
rm -rf "${PROJECT_DIR}/workers/unity/Library/"
rm -rf "${PROJECT_DIR}/workers/unity/Temp/"

${UNITY_EXE} \
    -nographics \
    -batchmode \
    -projectPath "${PROJECT_DIR}/workers/unity" \
    -runTests \
    -testPlatform playmode \
    -logfile "${PROJECT_DIR}/logs/unity-playmode-test-run.log" \
    -testResults "${PLAYMODE_TEST_RESULTS_FILE}"

PLAYMODE_TEST_RESULT=$?

markEndOfBlock "Playmode Testing"

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

if [ $EDITMODE_TEST_RESULT -ne 0 ] || [ $PLAYMODE_TEST_RESULT -ne 0 ] || [ $CODE_GENERATOR_TEST_RESULT -ne 0 ]; then
    exit 1
fi
