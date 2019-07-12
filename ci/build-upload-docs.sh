#!/usr/bin/env bash

### This script should only be run on Improbable's internal build machines.
### If you don't work at Improbable, this may be interesting as a guide to what software versions we use for our
### automation, but not much more than that.

set -e -u -x -o pipefail

# Build and upload a version of the docs.
#
# Environment variables as arguments:
#   DOCS_TYPE: "unity"

cd "$(dirname "$0")/../"

source ci/tools.sh

DOCS_PATH_TMP="$(mktemp -d)"
DOCS_PATH="./docs"
ENVIRONMENT="production"
ARTIFACT_FILE_NAME="improbadoc.upload.output.json"

setup_improbadoc "${ENVIRONMENT}"

echo $(date +%s%N | cut -b1-13) > "${DOCS_PATH}/timestamp.txt"

improbadoc build \
    "${DOCS_PATH}" \
    "${DOCS_PATH_TMP}" \
    --oauth2_client_cli_token_directory="${SPATIAL_OAUTH_DIR}"

improbadoc lint \
    "${DOCS_PATH_TMP}" \
    --oauth2_client_cli_token_directory="${SPATIAL_OAUTH_DIR}"

improbadoc upload \
    "${DOCS_TYPE}" \
    "${DOCS_PATH_TMP}" \
    --environment="${ENVIRONMENT}" \
    --oauth2_client_cli_token_directory="${SPATIAL_OAUTH_DIR}" \
    --json | jq '.' | tee "${ARTIFACT_FILE_NAME}"

HASH=$(jq -r .output.hash "${ARTIFACT_FILE_NAME}")

buildkite-agent meta-data set "docs-hash" "${HASH}"
