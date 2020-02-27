#!/usr/bin/env bash
set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
  set -x
fi

SHARED_CI_DIR="$(pwd)/.shared-ci"
CLONE_URL="git@github.com:spatialos/gdk-for-unity-shared-ci.git"
PINNED_SHARED_CI_BRANCH=$(cat ./ci/shared-ci.pinned | cut -d' ' -f 1)
PINNED_SHARED_CI_VERSION=$(cat ./ci/shared-ci.pinned | cut -d' ' -f 2)

if [[ -d "${SHARED_CI_DIR}" ]]; then
    rm -rf "${SHARED_CI_DIR}"
fi

mkdir "${SHARED_CI_DIR}"

# Clone the HEAD of the shared CI repo into ".shared-ci"
# Workaround for being unable to clone a specific commit with depth of 1.
pushd "${SHARED_CI_DIR}"
    git init
    git remote add origin "${CLONE_URL}"
    git fetch --depth 20 origin "${PINNED_SHARED_CI_BRANCH}"
    git checkout "${PINNED_SHARED_CI_VERSION}"
popd
