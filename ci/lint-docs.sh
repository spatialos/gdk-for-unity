#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

ci/bootstrap.sh

source ci/tools.sh

function annotations() {
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
    fi
}

DOCS_PATH="./docs"
ENVIRONMENT="production"

echo "Setting up Improbadoc linter"
fetch_service_account "${ENVIRONMENT}"
setup_improbadoc

echo "Running Improbadoc Linter"
improbadoc lint \
    "${DOCS_PATH}" \
    --oauth2_client_cli_token_directory="${SPATIAL_OAUTH_DIR}"

echo "Setting up Docs Linter"
setup_docs_linter

mkdir -p ./logs

echo "Running Docs Linter"
trap annotations ERR
docker run \
    -v $(pwd)/logs:/var/logs \
    local:gdk-docs-linter \
        "/var/logs/warnings.txt" \
        "/var/logs/errors.txt"

annotations