#!/usr/bin/env bash

set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

DOCGEN_PIN="${DOCGEN_OVERRIDE:-v1.0}"
CURRENT_DIR=$(pwd)
TMP_DIR=$(mktemp -d "${TMPDIR:-/tmp}/XXXXXXXXX")
OUTPUT_DIR="${CURRENT_DIR}/docs-output"
rm -rf "${OUTPUT_DIR}"
mkdir "${OUTPUT_DIR}"

function cleanUp() {
    # Ensure we are not in the temp dir before cleaning it
    cd "${CURRENT_DIR}"
    if [[ -n "${TMP_DIR-}" ]]; then
        rm -rf "${TMP_DIR}"
    fi
}

trap cleanUp EXIT

TAG=$(buildkite-agent meta-data get release-version)
CATEGORY_ID=$(buildkite-agent meta-data get category-id)

# Check if this tag is valid.
git rev-parse "${TAG}"

# Clone and checkout correct code tag.
CLONE_URL="git@github.com:spatialos/gdk-for-unity.git"
CODE_DIR="${TMP_DIR}/code"
git clone "${CLONE_URL}" "${CODE_DIR}" --branch "${TAG}" --depth 1

docker run --rm \
    -v "${CODE_DIR}/workers/unity/Packages:/input" \
    -v "${OUTPUT_DIR}:/output" \
    "eu.gcr.io/io-crafty-shelter/gdk-for-unity-docgen:${DOCGEN_PIN}" \
        --target-namespace="Improbable.Gdk" \
        --namespace-filter=".*EditmodeTests" \
        --namespace-filter=".*DeploymentLauncher" \
        --namespace-filter=".*PlaymodeTests" \
        --git-tag="${TAG}"

pushd "${OUTPUT_DIR}"
    buildkite-agent artifact upload "**/*.md"
popd

function generate_step() {
    echo "steps:"
    echo "  - trigger: platform-release-docs-readme"
    echo "    label: Upload documentation"
    echo "    build:"
    echo "      env:"
    echo "        bk_readme_project_name: gdk-for-unity"
    echo "        bk_readme_catergory_id: ${CATEGORY_ID}"
    echo "        bk_readme_version: ${TAG}"
    echo "        bk_readme_artifact_path: \"**/*\""
}

generate_step | buildkite-agent pipeline upload --no-interpolation
