#!/usr/bin/env bash

set -e -u -x -o pipefail

if [ ! "$#" -eq 1 ]; then
    echo "Expected usage: generate-docs.sh <git_hash>"
    exit 1
fi

TAG="${1}"

function cleanUp() {
    # Ensure we are not in the temp dir before cleaning it
    cd "${CURRENT_DIR}"
    if [[ -n "${TMP_DIR-}" ]]; then
        rm -rf "${TMP_DIR}"
    fi
}

trap cleanUp EXIT

cd "$(dirname "$0")/../"

# Make a copy of this repo and the docs repo.
CLONE_URL="git@github.com:spatialos/gdk-for-unity.git"
DOCGEN_CLONE_URL="git@github.com:improbable/gdk-for-unity-docgen.git"

CURRENT_DIR=$(pwd)
TMP_DIR=$(mktemp -d)
DOCGEN_DIR="${TMP_DIR}/docgen"
CODE_DIR="${TMP_DIR}/code"
DOCS_DIR="${TMP_DIR}/docs"

# Clone docgen repo.
git clone "${DOCGEN_CLONE_URL}" "${DOCGEN_DIR}"

# Clone and checkout correct code tag.
git clone "${CLONE_URL}" "${CODE_DIR}"
pushd "${CODE_DIR}"
    git checkout "${TAG}"
popd

# Clone and create branch for docs
DOCS_BRANCH="docs/api-docs-${TAG}"
git clone "${CLONE_URL}" "${DOCS_DIR}"
pushd "${DOCS_DIR}"
    git checkout docs-next
    if [ -n "$(git show-ref origin/${DOCS_BRANCH})" ]; then
        echo "Docs branch ${DOCS_BRANCH} already exists"
        exit 1
    fi
    git checkout -b "${DOCS_BRANCH}"
popd

# Generate API docs
dotnet run -p "${DOCGEN_DIR}/Docgen/Docgen.csproj" -- \
    --target-directory="${CODE_DIR}/workers/unity/Packages/" \
    --target-namespace="Improbable.Gdk" \
    --output-directory="${DOCS_DIR}/docs/api" \
    --namespace-filter=".*EditmodeTests" \
    --namespace-filter=".*DeploymentLauncher" \
    --namespace-filter=".*PlaymodeTests" \
    --api-path="api" \
    --git-tag="${TAG}"

# Commit and push
pushd "${DOCS_DIR}"
    git add --all
    git commit -m 'api docs'
    git push origin "${DOCS_BRANCH}"
popd

# If the hub CLI is on the command line, open a PR automatically.
if [ -x "$(command -v hub)" ]; then
    hub pull-request -b spatialos/gdk-for-unity:docs-next -h "spatialos/gdk-for-unity:${DOCS_BRANCH}" -o -m "API docs update for ${TAG}"
fi
