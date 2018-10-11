#!/usr/bin/env bash
set -e -u -x -o pipefail

cd "$(dirname "$0")/../"

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
    rm -rf ${TMP_DIR}
}

function fetchCloneUrl() {
    export $(cat .env | xargs)
    
    echo "git@${GITHUB_URL/.com\//.com:}"
    return 0
}

trap cleanUp EXIT

BRANCH_TO_TEST=$(git rev-parse --abbrev-ref HEAD)
CURRENT_DIR=$(pwd)

CLONE_URL=$(fetchCloneUrl)
TMP_DIR=$(mktemp -d)

# Go to a temporary directory and simulate the merge
pushd ${TMP_DIR}
    mkdir -p unity-gdk
    git clone ${CLONE_URL} unity-gdk
popd unity-gdk

git checkout master
git merge --no-commit --no-ff origin/${BRANCH_TO_TEST}
CHANGED_FILES=$(git diff HEAD --name-only)

NON_DOCS_FILES=()
for file_path in ${CHANGED_FILES} 
do
    if ! isDocsFile ${file_path} ; then
        NON_DOCS_FILES+=(${file_path})
    fi
done

if [ ${#NON_DOCS_FILES[@]} -ne 0 ]; then
    echo "Docs verification failed. The following files are not doc files:"
    for bad_file in "${NON_DOCS_FILES[@]}"
    do
        echo " * ${bad_file}"
    done
    exit 1
fi