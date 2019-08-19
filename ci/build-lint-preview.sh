#!/usr/bin/env bash

### This script should only be run on Improbable's internal build machines.
### If you don't work at Improbable, this may be interesting as a guide to what software versions we use for our
### automation, but not much more than that.

set -e -u -x -o pipefail

if [[ -z "$BUILDKITE" ]]; then
  echo "This script is only to be run on Improbable CI."
  exit 1
fi

cd "$(dirname "$0")/../"

ci/bootstrap.sh

source ci/tools.sh

function display_annotations() {
    WARNINGS_FILE="./logs/warnings.txt"
    if [[ -f "${WARNINGS_FILE}" ]]; then
        buildkite-agent annotate --style 'warning' --context 'ctx-warn' "<h2>Warnings:</h2><ul>"
        while read LINE; do
            buildkite-agent annotate --style 'warning' --context 'ctx-warn' --append "<li>${LINE}</li>"
        done < "${WARNINGS_FILE}"
        buildkite-agent annotate --style 'warning' --context 'ctx-warn' --append "</ul>"
    fi

    ERRORS_FILE="./logs/errors.txt"
    if [[ -f "${ERRORS_FILE}" ]]; then
        buildkite-agent annotate --style 'error' --context 'ctx-error' "<h2>Errors:</h2><ul>"
        while read LINE; do
            buildkite-agent annotate --style 'error' --context 'ctx-error' --append "<li>${LINE}</li>"
        done < "${ERRORS_FILE}"
        buildkite-agent annotate --style 'error' --context 'ctx-error' --append "</ul>"

        exit 1
    fi
}

DOCS_PATH_TMP="docs-build/"
DOCS_PATH="docs/"
ARTIFACT_FILE_NAME="improbadoc.upload.output.json"

setup_improbadoc

echo $(date +%s%N | cut -b1-13) > "${DOCS_PATH}/timestamp.txt"

echo "--- Building docs :hammer_and_wrench:"
improbadoc build \
    "${DOCS_PATH}" \
    "${DOCS_PATH_TMP}" \
    --oauth2_client_cli_token_directory="${SPATIAL_OAUTH_DIR}"

echo "--- Running Improbadoc Linter :lint-roller:"
improbadoc lint \
    "${DOCS_PATH_TMP}" \
    --oauth2_client_cli_token_directory="${SPATIAL_OAUTH_DIR}" \
    --enable_cross_lint=true \
    --doc_type="${DOCS_TYPE}" \
    --version="${DOCS_VERSION}"

setup_docs_linter "${DOCS_PATH_TMP}"

mkdir -p ./logs

echo "--- Running DocsLinter :link:"
trap display_annotations ERR
docker run \
    -v $(pwd)/logs:/var/logs \
    local:gdk-docs-linter \
        "/var/logs/warnings.txt" \
        "/var/logs/errors.txt"

display_annotations

echo "--- Uploading docs :outbox_tray:"
improbadoc upload \
    "${DOCS_TYPE}" \
    "${DOCS_PATH_TMP}" \
    --oauth2_client_cli_token_directory="${SPATIAL_OAUTH_DIR}" \
    --json | jq '.' | tee "${ARTIFACT_FILE_NAME}"

HASH=$(jq -r .output.hash "${ARTIFACT_FILE_NAME}")

buildkite-agent meta-data set "docs-hash" "${HASH}"

ci/tag-docs.sh
