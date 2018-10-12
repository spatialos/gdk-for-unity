#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

SHARED_CI_DIR="$(pwd)/.shared-ci"
CLONE_URL="git@github.com:spatialos/gdk-for-unity-shared-ci.git"

# Clone the HEAD of the shared CI repo into ".shared-ci"

if [ -d "${SHARED_CI_DIR}" ]; then
    rm -rf "${SHARED_CI_DIR}"
fi

git clone "${CLONE_URL}" "${SHARED_CI_DIR}"
