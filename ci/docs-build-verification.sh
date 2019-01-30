#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

function isDocsBranch() {
  if [[ -n "${BUILDKITE_BRANCH-}" ]]; then
    BRANCH="${BUILDKITE_BRANCH}"
  else
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
  fi

  if [[ "${BRANCH}" == docs/* ]]; then
    return 0
  fi
  return 1
}

function isDocsFile() {
    FILE_PATH=${1}

    if echo ${FILE_PATH} | grep "docs/" ; then
        return 0
    fi

    if echo ${FILE_PATH} | grep ".*.md" ; then
        return 0
    fi
    
    return 1
}

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

        # TODO: If buildkite set local git info.

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