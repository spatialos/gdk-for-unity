#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

SHARED_CI_DIR="$(pwd)/.shared-ci"
CLONE_URL="git@github.com:spatialos/gdk-for-unity-shared-ci.git"

# Clone the HEAD of the shared CI repo into ".shared-ci"

if [[ -d "${SHARED_CI_DIR}" ]]; then
    rm -rf "${SHARED_CI_DIR}"
fi

git config --local remote.origin.receivepack "powershell git receive-pack"
git config --local remote.origin.uploadpack "powershell git upload-pack"

env | sort
git config --global --includes --list | sort
GIT_TRACE=1 GIT_TRACE_PACKET=1 GIT_TRACE_SETUP=1 GIT_CURL_VERBOSE=1 GIT_SSH_COMMAND='c:/Program\ Files/OpenSSH-Win64/ssh.exe -vvv' git clone --verbose --depth 1 "${CLONE_URL}" "${SHARED_CI_DIR}"
