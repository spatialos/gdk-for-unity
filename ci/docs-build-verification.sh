#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

function cleanUp() {
    # Ensure we are not in the temp dir before cleaning it
    cd ${CURRENT_DIR}
    if [[ -n "${TMP_DIR-}" ]]; then
        rm -rf ${TMP_DIR}
    fi
}

function fetchCloneUrl() {
    export $(cat .env | xargs)
    
    echo "git@${GITHUB_URL/.com\//.com:}"
    return 0
}

ci/bootstrap.sh
source .shared-ci/scripts/pinned-tools.sh

trap cleanUp EXIT

CURRENT_DIR=$(pwd)

if ! isDocsBranch; then
    exit 0
fi

CURRENT_COMMIT=$(git rev-parse HEAD)

CLONE_URL=$(fetchCloneUrl)
TMP_DIR=$(mktemp -d)

# Go to a temporary directory and simulate the merge
pushd ${TMP_DIR}

    mkdir -p unity-gdk
    git clone ${CLONE_URL} unity-gdk

    pushd unity-gdk

        if [[ -n "${BUILDKITE_BRANCH-}" ]]; then
            git config user.email "need-email-for-fake-merge@improbable.io"
            git config user.name "Fake Merge"
        fi

        git checkout develop
        git merge --no-commit --no-ff ${CURRENT_COMMIT}
        CHANGED_FILES=$(git diff HEAD --name-only)

        NON_DOCS_FILES=()
        for file_path in ${CHANGED_FILES} 
        do
            if ! isDocsFile ${file_path} ; then
                NON_DOCS_FILES+=(${file_path})
            fi
        done
        
    popd

popd 


if [ ${#NON_DOCS_FILES[@]} -ne 0 ]; then
    echo "Docs verification failed. The following files are not doc files:"
    for bad_file in "${NON_DOCS_FILES[@]}"
    do
        echo " * ${bad_file}"
    done
    exit 1
fi